﻿using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
	public class AutorCreacionDTO
	{
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener mas de {1} carácter")]
		[PrimeraLetraMayuscula]
		public string Nombre { get; set; }
    }
}
