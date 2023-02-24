using System;
using XWMessageQueue;
namespace LostCivilization.World
{
    public abstract class AWorldController : IWorldController, IGameEventListener<WorldMessage>
    {
        protected IWorldModel _model;
        protected AWorldContext _context;

        public  void Init(IWorldModel worldModel = null)
        {
            _model = worldModel ?? GetInitModel();
            _context = GetInitContext();
            InitControllers();
            WorldMessageQueue.Instance.Subscribe(this,WorldMessage.Update,WorldMessage.Stop,WorldMessage.Run);
            WorldUIMessageQueue.SendGameEventS(WorldUIMessage.Init);
        }

        protected abstract void InitControllers();

        protected abstract AWorldContext GetInitContext();

        /// <summary>
        /// 初始化数据
        /// </summary>
        protected abstract IWorldModel GetInitModel();
        
        private void DoUpdate()
        {
            Act();
            WorldUIMessageQueue.SendGameEventS(WorldUIMessage.RefreshUI);
        }
        /// <summary>
        /// 执行一次逻辑
        /// </summary>
        public abstract void Act();
        public void Run()
        {
            _context.state = 1;
            WorldUIMessageQueue.SendGameEventS(WorldUIMessage.Run);
        }
        public void Stop()
        {
        }
        public IWorldModel GetModel()
        {
            return _model;
        }
        public AWorldContext GetContext()
        {
            return _context;
        }
        public virtual void OnMessage(GameEvent<WorldMessage> inGameEvent)
        {
            switch (inGameEvent.operate) {
                case WorldMessage.Update:
                    DoUpdate();
                    break;
                case WorldMessage.Run:
                    Run();
                    DoUpdate();
                    break;
                case WorldMessage.Stop:
                    Dispose();
                    break;
                default:
                    break;
            }
            WorldMessageQueue.Instance.Callback();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are.
        ~AWorldController()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                WorldMessageQueue.Instance.UnSubscribe(this, WorldMessage.Update,WorldMessage.Stop,WorldMessage.Run);
            }
            // free native resources here if there are any
        }
    }
}