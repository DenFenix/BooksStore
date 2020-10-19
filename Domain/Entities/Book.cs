using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue =false)]
        public int BookId { get; set; }

        [Display(Name ="Автор")]
        [Required(ErrorMessage = "Пожалуйста, имя автора")]
        public string Author { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название Книги")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите описание для Книги")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию для Книги")]
        public string Category { get; set; }

        [Display(Name ="Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
    }
}
