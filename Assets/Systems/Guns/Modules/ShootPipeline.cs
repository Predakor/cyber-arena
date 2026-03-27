using System.Collections.Generic;
using System.Linq;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns.Modules
{
    public sealed class ShootPipeline
    {
        private readonly IReadOnlyList<IGunModule> _modules;

        public ShootPipeline(params IGunModule[] modules)
        {
            _modules = modules.Where(m => m != null).ToArray();
        }

        public void Execute(ShootContext context)
        {
            int index = 0;
            void Next(ShootContext ctx)
            {
                if (index < _modules.Count)
                {
                    _modules[index++].Handle(ctx, Next);
                }
            }
            Next(context);
        }
    }
}