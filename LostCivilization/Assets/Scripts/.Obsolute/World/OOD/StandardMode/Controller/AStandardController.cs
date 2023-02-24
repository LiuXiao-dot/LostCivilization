using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 子规则控制器
    /// </summary>
    public abstract class AStandardController : IController
    {
        protected StandardWorldContext _context;
        
        public AStandardController(StandardWorldContext context)
        {
            this._context = context;
        }

        public abstract void Act();
    }
}