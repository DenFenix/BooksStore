using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PagingInfo
    {
        /// <summary>
        /// Кол-во товаров
        /// </summary>
        public int TotalItlems { get; set; }

        /// <summary>
        /// Кол-во товаров на одной странице
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Общее кол-во страниц
        /// </summary>
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItlems / ItemsPerPage); }
        }
    }
}