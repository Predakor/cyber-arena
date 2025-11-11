namespace Systems.Weapons.Guns.Modules {
    public interface IFireRequest { }

    public interface IGunModule {
        IGunModule Handle(IFireRequest request);
    }
}
