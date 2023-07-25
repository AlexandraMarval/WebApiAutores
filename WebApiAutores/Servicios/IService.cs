using static WebAPIAutores.Servicios.ServicioA;
using static WebAPIAutores.Servicios.ServicioB;

namespace WebAPIAutores.Servicios
{
    public interface IService
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }

    public class ServicioA : IService
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ServicioScoped servicioScoped;

        public ServicioA(ILogger<ServicioA> logger, ServicioTransient servicioTransient, ServicioSingleton servicioSingleton, ServicioScoped servicioScoped)
        {
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioSingleton = servicioSingleton;
            this.servicioScoped = servicioScoped;
        }

        public Guid ObtenerTransient() { return servicioTransient.Guid; }
        public Guid ObtenerScoped() { return servicioScoped.Guid; }
        public Guid ObtenerSingleton() { return servicioSingleton.Guid; }

        public void RealizarTarea()
        {

        }
    }

    public class ServicioB : IService
    {
        public class ServicioTransient
        {
            public Guid Guid = Guid.NewGuid();
        }

        public class ServicioScoped
        {
            public Guid Guid = Guid.NewGuid();
        }
        public class ServicioSingleton
        {
            public Guid Guid = Guid.NewGuid();
        }
        public void RealizarTarea()
        {

        }

        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }
    }

}
