using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.HTMLHelpers
{
    public static class PagingHelpers
    {
        //в хклпере в качестве перого параметра объект для которого создается метод
        /// <summary>
        /// генерирует HTML-разметку для набора ссылок на страницы
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int,string> pageUrl)
        {
            //формируем объект для текста
            StringBuilder result = new StringBuilder();
            //проходимся по каждой странице
            for(int i = 1; i<=pagingInfo.TotalPages;i++)
            {
                //с помощью TagBuilder формируем необходимый элемент
                TagBuilder tag = new TagBuilder("a");
                //добавляем к тегу атрибут
                tag.MergeAttribute("href", pageUrl(i));
                //добавляем содержимое тега в виде строки
                tag.InnerHtml = i.ToString();

                if(i == pagingInfo.CurrentPage)
                {
                    //добавляет класс css к элементу
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                //собираем все теги в result
                result.Append(tag.ToString());
            }
            //создаём строку в html исрльзуя текст
            return MvcHtmlString.Create(result.ToString());
        }
    }
}