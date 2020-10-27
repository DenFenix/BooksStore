using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Json;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver:IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBings();
        }
        /// <summary>
        /// Указываем, какие объекты для кого интерфейса применяются
        /// </summary>
        private void AddBings()
        {
            //тут будет привязки
            //kernel.Bind<IXXX>().To<XXX>();
            /* Mock<IBookRepository> mock = new Mock<IBookRepository>();
             //настраиваем мок
             mock.Setup(m => m.Books).Returns(new List<Book>
             {
                 new Book {Name = "451 градус по фарингейту", Price = 1500},
                 new Book {Name = "1984", Price = 800},
                 new Book {Name = "Бакуман", Price = 1101.5M}
             });*/
            //Связываем интерфейс и созданный объект мока
            //ToConstant возращает один и тот же объект
            //kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            if(bool.Parse(ConfigurationManager.AppSettings["IsJson"]))           
                kernel.Bind<IBookRepository>().To<JSONBookRepository>();          
            else
                kernel.Bind<IBookRepository>().To<EFBookeRepository>();
            

            EmailSettings emailSettings = new EmailSettings
            {
                //ConfigurationManager - настройка из web.config свойства appsettigs, строки WriteAsFile
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}