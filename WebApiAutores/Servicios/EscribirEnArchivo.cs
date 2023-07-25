namespace WebAPIAutores.Servicios
{
	public class EscribirEnArchivo : IHostedService
	{
		// Ejecutar  Codigo Recurrente con IHostedService
		private readonly IHostEnvironment environment;
		private readonly string nombreArchivo = "Archivo 1.txt";
		private Timer timer;

		public EscribirEnArchivo(IHostEnvironment environment)
        {
			this.environment = environment;
		}

        public Task StartAsync(CancellationToken cancellationToken)
		{
			timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
			Escribir("Proceso iniciado");
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			timer.Dispose();
			Escribir("Proceso finalizado");
			return Task.CompletedTask;
		}
		private void DoWork(object state)
		{
			Escribir("Proceso en ejecución" + DateTime.Now.ToString("dd/mm/yyyy hh:mm:ss"));
		}

		private void Escribir(string mensaje)
		{
			var ruta = $@"{environment.ContentRootPath}\wwwroot\{nombreArchivo}";
			using (StreamWriter writer = new StreamWriter(ruta, append: true))
			{
				writer.WriteLine(mensaje);
			}
		}
	}
}
