namespace O2.Flux {
    public interface ITickable {
        void Tick();
    }

    public interface IFixedTickable {
        void FixedTick();
    }

    public interface ILateTickable {
        void LateTick();
    }

    public interface IDestroyable {
        void OnDestroy();
    }
}