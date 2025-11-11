namespace Systems.Guns {
    public interface IWeapon { };
    public interface IGun : IWeapon {
        void Use();
    }
}
