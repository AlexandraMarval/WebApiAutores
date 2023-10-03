using Microsoft.Extensions.Options;

namespace WebApiAutores.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        public int recordsPorPagina = 10;
        private readonly int CantidaMaximaPorPagina = 50;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > CantidaMaximaPorPagina) ? CantidaMaximaPorPagina : value;
            }
        }
    }
}
