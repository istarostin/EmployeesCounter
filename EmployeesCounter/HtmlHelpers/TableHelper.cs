using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeesCounter.Models;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.Encodings.Web;

namespace EmployeesCounter.HtmlHelpers
{
    public static class TableHelper
    {
        public static HtmlString CreateTable(this IHtmlHelper html, List<EmployeeViewModel> viewModel,
            TableModel tableModel, Func<int, string> editUrl, Func<int, string> deleteUrl, Func<int, string> tableUrl)
        {
            StringBuilder tableWithLinks = new StringBuilder("<div>");
            StringBuilder table = new StringBuilder("<table>");
            for (int j = 1; j <= tableModel.TotalTables; j++)
            {
                TagBuilder linkTag = new TagBuilder("a"); // Construct an <a> tag
                linkTag.MergeAttribute("href", tableUrl(j));
                linkTag.MergeAttribute("class", "tableLink");
                linkTag.InnerHtml.SetContent(String.Format("Страница "+j.ToString()));
                if (j == tableModel.CurrentTable)
                    linkTag.AddCssClass("selected");
                var linkWriter = new System.IO.StringWriter();
                linkTag.WriteTo(linkWriter, HtmlEncoder.Default);
                tableWithLinks.Append(linkWriter.ToString());
            }
            table.Append("<thead>" +
                "<tr>" +
                "<td>Имя</td>" +
                "<td>Фамилия</td>" +
                "<td>Возраст</td>" +
                "<td>Отдел</td>" +
                "<td>Язык</td>" +
                "<td>Действия</td>" +
                "</tr>" +
                "</thead>" +
                "<tbody>");
            for (int i = (tableModel.CurrentTable - 1) * tableModel.ElementPerTable; i < (tableModel.CurrentTable) * tableModel.ElementPerTable; i++)
            {
                if (i < viewModel.Count)
                {
                    TagBuilder editA = new TagBuilder("a");
                    TagBuilder deleteA = new TagBuilder("a");
                    editA.MergeAttribute("href", editUrl(viewModel[i].EmpId));
                    editA.InnerHtml.SetContent("Изменить");
                    deleteA.MergeAttribute("href", deleteUrl(viewModel[i].EmpId));
                    deleteA.InnerHtml.SetContent("Удалить");
                    table.Append("<tr>" +
                        "<td>" + viewModel[i].Name + "</td>" +
                        "<td>" + viewModel[i].Surname + "</td>" +
                        "<td>" + viewModel[i].Age + "</td>" +
                        "<td>" + viewModel[i].DepartmentTitle + "</td>" +
                        "<td>" + viewModel[i].ProgLangTitle + "</td>" +
                        "<td>"
                        );
                    var editWriter = new System.IO.StringWriter();
                    editA.WriteTo(editWriter, HtmlEncoder.Default);
                    table.Append(editWriter.ToString());
                    var deleteWriter = new System.IO.StringWriter();
                    deleteA.WriteTo(deleteWriter, HtmlEncoder.Default);                    
                    table.Append(deleteWriter.ToString());                       
                    table.Append("</td>" + "</tr>");
                }
            }
            table.Append("</tbody></table>");
            tableWithLinks.Append(table);
            tableWithLinks.Append("</div>");
            return new HtmlString(tableWithLinks.ToString());
        }
    }


}
