using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Ifrastructure.Binder
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";
        /// <summary>
        /// обеспечить возможность создания объекта модели предметной области
        /// </summary>
        /// <param name="controllerContext">обеспечивает доступ ко всей информации, которой располагает класс контроллера</param>
        /// <param name="bindingContext"></param>
        /// <returns>Предоставляет сведения об объекте модели, который требуется создать</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if(controllerContext.HttpContext.Session!=null)
            {
                //войство HttpContext, которое, в свою очередь, содержит свойство Session, позволяющее получать и устанавливать данные сеанса
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            if(cart ==null)
            {
                cart = new Cart();
                if(controllerContext.HttpContext.Session!=null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}