using CreativeCoders.Core.Messaging;
using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm
{
    [PublicAPI]
    public class ViewModelBase<TModel> : ViewModelBase where TModel: class
    {
        public ViewModelBase(TModel model)
        {
            Model = model;
            ModelLink = new ObjectLinkBuilder(model, this).Build();
        }

        public ViewModelBase(TModel model, IMessenger messenger) : this(model)
        {
            Messenger = messenger;
        }

        protected TModel Model { get; }

        protected ObjectLink ModelLink { get; }
    }
}
