using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Printer.Web.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("aria-label", "Page navigation");
            output.Content.SetHtmlContent(AddPageContent());
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageRange { get; set; }
        public string PageFirst { get; set; }
        public string PageLast { get; set; }
        public string PageTarget { get; set; }

        private string AddPageContent()
        {
            if (PageRange == 0)
            {
                PageRange = 1;
            }

            if (PageCount < PageRange)
            {
                PageRange = PageCount;
            }

            if (string.IsNullOrEmpty(PageFirst))
            {
                PageFirst = "First";
            }

            if (string.IsNullOrEmpty(PageLast))
            {
                PageLast = "Last";
            }

            var content = new StringBuilder();
            content.Append("<div class='col-lg-12'>");
            content.Append("<div class='pagination-area'>");
            content.Append("<nav aria-label='Page navigation example' class='text-center'>");
            content.Append("<ul class='pagination justify-content-center'>");

            if (PageNumber > 1)
            {
                content.Append("<li class='page-item'><a class='page-link page-links' href='" + PageTarget + "?p=" + (PageNumber - 1) + "'><i class='bx bx-chevron-left'></i></a></li>");
            }
            else
            {
                content.Append("<li class='page-item disabled'><a class='page-link page-links' href='#'><i class='bx bx-chevron-left'></i></a></li>");
            }

            for (int currentPage = Math.Max(1, PageNumber - PageRange); currentPage <= Math.Min(PageCount, PageNumber + PageRange); currentPage++)
            {
                var active = currentPage == PageNumber ? " active" : "";
                content.Append("<li class='page-item" + active + "'><a class='page-link' href='" + PageTarget + "?p=" + currentPage + "'>" + currentPage + "</a></li>");
            }

            if (PageNumber < PageCount)
            {
                content.Append("<li class='page-item'><a class='page-link page-links' href='" + PageTarget + "?p=" + (PageNumber + 1) + "'><i class='bx bx-chevron-right'></i></a></li>");
            }
            else
            {
                content.Append("<li class='page-item disabled'><a class='page-link page-links' href='#'><i class='bx bx-chevron-right'></i></a></li>");
            }

            content.Append("</ul>");
            content.Append("</nav>");
            content.Append("</div>");
            content.Append("</div>");

            return content.ToString();
        }



    }
}
