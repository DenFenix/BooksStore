using Domain.Abstract;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //настраиваем мок
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {Name = "451", Price = 1500},
                new Book {Name = "1984", Price = 800},
                new Book {Name = "1945", Price = 1101.5M}
            });
            //Связываем интерфейс и созданный объект мока
            kernel.Bind<IBookRepository>().ToConstant(mock.Object);
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