using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BlogSystem.BLL;
using BlogSystem.DAL;
using BlogSystem.IBLL;
using BlogSystem.IDAL;

namespace BlogSystem.MVCSite
{
    public class AutofacConfig
    {
        public static void Register()
        {
            //配置autofac的基本信息
            //(1)创建容器
            var builder = new ContainerBuilder();
            //(2)进行依赖注入的注册
            //builder.RegisterType<RolesDal>().As<IRolesDal>();
            //builder.RegisterType<RolesBll>().As<IRolesBll>();
            //通过反射来实现直接配置程序集
            Assembly dal = Assembly.Load("BlogSystem.DAL"); //通过反射来找到对应的dal层
            builder.RegisterAssemblyTypes(dal).AsImplementedInterfaces(); //通过容器注册反射等得到类型，并且与接口进行关联
            
            Assembly bll = Assembly.Load("BlogSystem.BLL");
            builder.RegisterAssemblyTypes(bll).AsImplementedInterfaces();



            //(3)注册控制器
            builder.RegisterControllers(typeof(AutofacConfig).Assembly);
            //(4)构建
            var container = builder.Build();
            //(5)实现
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}