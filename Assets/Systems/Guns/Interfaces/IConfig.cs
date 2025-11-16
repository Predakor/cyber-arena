namespace Systems.Guns.Interfaces {
    public interface IConfig { };
    public interface IConfig<TFor> : IConfig { }
    public interface IConfigurable<TConfig> where TConfig : IConfig {
        void Configure(TConfig config);
    }
}
