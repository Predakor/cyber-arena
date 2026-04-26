using System;
using System.Linq;
using Systems.Guns.Modules.Shared;
using Systems.Weapons.Guns.Modules;

namespace Systems.Guns.Modules
{
    public sealed class ShootPipeline
    {
        private readonly Action<ShootContext> _chain;

        public ShootPipeline(params IGunModule[] modules)
        {
            _chain = Build(modules.Where(m => m != null).ToArray(), 0);
        }

        public void Execute(ShootContext context) => _chain(context);

        private static Action<ShootContext> Build(IGunModule[] modules, int index)
        {
            if (index >= modules.Length)
            {
                return _ => { };
            }
            var downstream = Build(modules, index + 1);
            return ctx => modules[index].Handle(ctx, downstream);
        }
    }
}