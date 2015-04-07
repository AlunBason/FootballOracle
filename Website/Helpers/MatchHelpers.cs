using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using System.Web.Mvc.Html;
using FootballOracle.Models.ViewModels.Approvable.Matches;

namespace System.Web.Mvc
{
    public static class MatchHelpers
    {
        

        public static IHtmlString ToMatchList(this HtmlHelper html, string titleDescription, IEnumerable<IGrouping<DateTime?, BaseMatchViewModel>> matchViewModels, int pageNumber, int groupsPerPage, bool isInAscendingOrder)
        {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var htmlTextWriter = new HtmlTextWriter(stringWriter);

            var matchGroups = isInAscendingOrder
                ? matchViewModels.OrderBy(m => m.FirstOrDefault().MatchDate)
                : matchViewModels.OrderByDescending(m => m.FirstOrDefault().MatchDate);

            var table = new Table() { CssClass = "table table-striped table-condensed table-hover small" };
            table.Rows.Add(CreateHeaderRow(titleDescription));

            foreach (var group in matchGroups.Skip((pageNumber - 1) * groupsPerPage).Take(groupsPerPage))
            {
                var tr = new TableRow() { CssClass = "success" };
                tr.Cells.Add(new TableHeaderCell() { ColumnSpan = 3, CssClass = "text-center", Text = group.FirstOrDefault().MatchDate.ToDisplayString()});
                table.Rows.Add(tr);

                foreach (var item in group.OrderBy(g => g.Team1ViewModel.ToString()))
                {
                    tr = new TableRow() { TableSection = TableRowSection.TableBody };
                    tr.Cells.Add(new TableCell()
                    {
                        CssClass = "text-right",
                        Width = new Unit(46, UnitType.Percentage),
                        Text = html.DisplayBadgeWithTeamLink(item.VersionEntity.Team1, item.MatchDate, 20, 20, null, true).ToString()
                    });

                    tr.Cells.Add(new TableCell()
                    {
                        CssClass = "text-center",
                        Width = new Unit(8, UnitType.Percentage),
                        Text = item.Team1Ft != null && item.Team2Ft != null
                            ? html.ApprovalTypeLink(AreaType.Mtc, string.Format("{0} : {1}", item.Team1Ft.ToString(), item.Team2Ft.ToString()), item.HeaderKey, DateTime.Today).ToString()
                            : html.ApprovalTypeLink(AreaType.Mtc, " v ", item.HeaderKey, item.MatchDate).ToString()
                    });

                    tr.Cells.Add(new TableCell()
                    {
                        Width = new Unit(46, UnitType.Percentage),
                        Text = html.DisplayBadgeWithTeamLink(item.VersionEntity.Team2, item.MatchDate, 20, 20, null).ToString()
                    });


                    table.Rows.Add(tr);
                }
            }

            table.RenderControl(htmlTextWriter);

            return new HtmlString(stringBuilder.ToString());
        }

        private static TableHeaderRow CreateHeaderRow(string titleDescription)
        {
            var header = new TableHeaderRow() { TableSection = TableRowSection.TableHeader };

            header.Cells.Add(CreateHeaderCell("text-center", titleDescription));

            return header;
        }

        private static TableHeaderCell CreateHeaderCell(string classValue, string text)
        {
            return new TableHeaderCell() { CssClass = classValue, Text = text, ColumnSpan = 3 };
        }
    }
}