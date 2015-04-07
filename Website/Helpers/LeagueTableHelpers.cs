using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using FootballOracle.Models.ViewModels.Standard;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;


namespace System.Web.Mvc
{
    public static class LeagueTableHelpers
    {
        public static IHtmlString ToHtmlLeagueTable(this HtmlHelper html, BaseCampaignViewModel campaignViewModel, Guid? teamVPrimaryKey, DateTime viewDate, int viewType, bool showForm = false)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);

            var table = new Table() { CssClass = "table table-striped table-condensed table-hover small" };

            table.Rows.Add(CreateCampaignDescriptionRow(campaignViewModel, viewDate));
            table.Rows.Add(CreateFullHeaderRow(showForm));

            if (campaignViewModel.SelectedCampaignStageViewModel != null)
                foreach (var item in campaignViewModel.SelectedCampaignStageViewModel.LeagueTableItemViewModels)
                    table.Rows.Add(html.CreateFullRow(item, item.TeamViewModel.VersionEntity.PrimaryKey == teamVPrimaryKey, viewDate, viewType, showForm));

            table.RenderControl(htmlTextWriter);

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlString ToHtmlLeagueTable(this HtmlHelper html, IEnumerable<LeagueTableItemViewModel> leagueTableItemViewModels, DateTime viewDate, bool showForm = false)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);

            var table = new Table() { CssClass = "table table-striped table-condensed table-hover small" };

            table.Rows.Add(CreateFullHeaderRow(showForm));

            foreach (var item in leagueTableItemViewModels)
                table.Rows.Add(html.CreateFullRow(item, false, viewDate, 3, showForm));


            table.RenderControl(htmlTextWriter);

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlString ToHtmlCutDownLeagueTable(this HtmlHelper html, BaseCampaignViewModel campaignViewModel, Guid? teamVPrimaryKey, DateTime viewDate)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);

            var table = new Table() { CssClass = "table table-striped table-condensed table-hover small" };

            if (campaignViewModel != null)
            {
                var title = html.ApprovalTypeLink(AreaType.Cmp, campaignViewModel.CampaignDisplayWithCompetition, campaignViewModel.Entity.Competition.GetApprovedVersion<CompetitionV>(viewDate).HeaderKey, viewDate).ToString();
                table.Rows.Add(CreateTitle(title, 4));
            }

            table.Rows.Add(CreateCutDownHeaderRow());

            foreach (var item in campaignViewModel.SelectedCampaignStageViewModel.LeagueTableItemViewModels)
                table.Rows.Add(html.CreateCutDownRow(item, item.TeamViewModel.VersionEntity.PrimaryKey == teamVPrimaryKey, viewDate));

            table.RenderControl(htmlTextWriter);

            return new HtmlString(stringBuilder.ToString());
        }

        private static TableHeaderRow CreateTitle(string title, int colspan)
        {
            var header = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };
            header.Cells.Add(new TableHeaderCell()
            {
                CssClass = "text-center success",
                ColumnSpan = colspan,
                Text = title
            });

            return header;
        }

        private static TableHeaderRow CreateFullHeaderRow(bool showForm = false)
        {
            var header = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };

            header.Cells.Add(CreateHeaderCell(string.Empty, string.Empty));
            header.Cells.Add(CreateNoWrapHeaderCell(string.Empty, string.Empty, "40%"));
            header.Cells.Add(CreateHeaderCell("text-center", "P", toolTip: "Played"));
            header.Cells.Add(CreateHeaderCell("text-center", "W", toolTip: "Won"));
            header.Cells.Add(CreateHeaderCell("text-center", "D", toolTip: "Drawn"));
            header.Cells.Add(CreateHeaderCell("text-center", "L", toolTip: "Lost"));
            header.Cells.Add(CreateHeaderCell("text-center", "F", toolTip: "Goals for"));
            header.Cells.Add(CreateHeaderCell("text-center", "A", toolTip: "Goals against"));
            header.Cells.Add(CreateHeaderCell("text-center", "Pts", toolTip: "Points"));

            if (showForm)
                header.Cells.Add(CreateHeaderCell("text-center bg-info", "Form", toolTip: "This is an index of the number of points scored where the most recent matches count more towards the total than older matches."));

            return header;
        }

        private static TableHeaderRow CreateCampaignDescriptionRow(BaseCampaignViewModel campaignViewModel, DateTime viewDate)
        {
            var header = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };

            var campaignDisplay = viewDate.Date < campaignViewModel.Entity.EndDate.Date && viewDate.Date < DateTime.Today
                ? string.Format("{0} at {1}", campaignViewModel.CampaignDisplayWithCompetition, viewDate.ToShortDateString())
                : campaignViewModel.CampaignDisplayWithCompetition;

            header.Cells.Add(CreateHeaderCell("text-center", campaignDisplay, 10));

            return header;
        }

        private static TableHeaderRow CreateCutDownHeaderRow()
        {
            var header = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };

            header.Cells.Add(CreateHeaderCell(string.Empty, string.Empty));
            header.Cells.Add(CreateHeaderCell("text-center", "P"));
            header.Cells.Add(CreateHeaderCell("text-center", "Pts"));
            header.Cells.Add(CreateHeaderCell("text-center", "GD"));

            return header;
        }

        private static TableHeaderCell CreateHeaderCell(string classValue, string text, int columnSpan = 1, string toolTip = "")
        {
            return new TableHeaderCell() { CssClass = classValue, Text = text, ColumnSpan = columnSpan, ToolTip = toolTip };
        }

        private static TableHeaderCell CreateNoWrapHeaderCell(string classValue, string text, string width, int columnSpan = 1)
        {
            return new TableHeaderCell() { CssClass = classValue, Text = text, ColumnSpan = columnSpan, Wrap = false, Width = new Unit(width) };
        }

        private static TableRow CreateFullRow(this HtmlHelper html, LeagueTableItemViewModel value,  bool isHighlighted, DateTime viewDate, int viewType, bool showForm = false)
        {
            var row = new TableRow() { TableSection = TableRowSection.TableBody, CssClass = isHighlighted ? "info" : string.Empty };

            row.Cells.Add(CreateCell("text-center", value.Position.ToString()));
            row.Cells.Add(CreateCell(string.Empty, html.DisplayBadgeWithTeamLink(value.TeamViewModel.VersionEntity.Team, viewDate, 20, 20, null).ToString()));
            row.Cells.Add(CreateCell("text-center", value.Played.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Wins.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Draws.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Losses.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Scored.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Conceded.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Points.ToString()));

            if (showForm)
            {
                switch (viewType)
                {
                    case 1:
                        row.Cells.Add(CreateCell("text-center bg-info", string.Format("{0:P0}", value.TeamViewModel.HomeLeagueForm)));
                        break;

                    case 2:
                        row.Cells.Add(CreateCell("text-center bg-info", string.Format("{0:P0}", value.TeamViewModel.AwayLeagueForm)));
                        break;

                    default:
                        row.Cells.Add(CreateCell("text-center bg-info", string.Format("{0:P0}", value.TeamViewModel.LeagueForm)));
                        break;
                }
            }
            
            return row;
        }

        private static TableRow CreateCutDownRow(this HtmlHelper html, LeagueTableItemViewModel value, bool isHighlighted, DateTime viewDate)
        {
            var row = new TableRow() { TableSection = TableRowSection.TableBody, CssClass = isHighlighted ? "info" : string.Empty };

            row.Cells.Add(CreateCell(string.Empty, html.DisplayBadgeWithTeamLink(value.TeamViewModel.VersionEntity.Team, viewDate, 20, 20, null).ToString()));
            row.Cells.Add(CreateCell("text-center", value.Played.ToString()));
            row.Cells.Add(CreateCell("text-center", value.Points.ToString()));
            row.Cells.Add(CreateCell("text-center", value.GoalDifference.ToString("+#;-#;0")));
            return row;
        }

        private static TableCell CreateCell(string classValue, string text)
        {
            return new TableCell() { CssClass = classValue, Text = text };
        }
    }
}
