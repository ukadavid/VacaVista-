using System;
using VecaVista.Model.Dto;

namespace VecaVista.Data
{
	public class DataStore
	{
        public static List<VecaDto> vecaList = new List<VecaDto>
        {
            new VecaDto
                {
                    Id = 1,
                    Name = "Lagos"
                },
                new VecaDto
                {
                    Id = 2,
                    Name = "Asaba"
                }

        };
    }
}

