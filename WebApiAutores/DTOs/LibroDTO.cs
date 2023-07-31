﻿using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
	public class LibroDTO
	{
		public int Id { get; set; }
		[PrimeraLetraMayuscula]
		[StringLength(maximumLength: 250)]
		public string Titulo { get; set; }
	}
}
