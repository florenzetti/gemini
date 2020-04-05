//using Gemini.Framework.Services;
//using Gemini.Modules.Explorer.ViewModels;
//using Gemini.Framework;
//using Caliburn.Micro;
//using System.Threading.Tasks;
//using System.Threading;
//using System;
//using System.Windows;

//namespace Gemini.Modules.Explorer
//{
//    public abstract class ExplorerTool<TExplorerProvider> : Tool
//        where TExplorerProvider : class, IExplorerProvider
//    {
//        public abstract bool UseDefaultView { get; }
//        public TExplorerProvider GetProvider()
//        {
//            return IoC.Get<TExplorerProvider>();
//        }

//        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
//        {
//            if (UseDefaultView)
//            {
//                var defaultFunc = ViewLocator.LocateForModelType;
//                ViewLocator.LocateForModelType = (Type arg1, DependencyObject arg2, object arg3) =>
//                {
//                    var targetType = arg1;
//                    if (arg1 == GetType())
//                        targetType = typeof(ExplorerViewModel);
//                    return defaultFunc(targetType, arg2, arg3);
//                };
//            }

//            return base.OnInitializeAsync(cancellationToken);
//        }
//    }
//}
