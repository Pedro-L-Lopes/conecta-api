namespace conecta_api.Pagination
{
    public class PropertyParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        // Parâmetros de ordenação
        public string? OrderBy { get; set; } = "CreatedAt"; // Campo padrão para ordenação
        public string? SortDirection { get; set; } = "asc"; // Direção padrão

        // Parâmetros de filtro
        public string? City { get; set; } // Filtro por cidade
        public string? Neighborhood { get; set; } // Filtro por bairro
        public int? Bedrooms { get; set; } // Filtro por número de quartos
        public int? ParkingSpaces { get; set; } // Filtro por número de vagas
        public decimal? MinPrice { get; set; } // Filtro por faixa de preço (mínimo)
        public decimal? MaxPrice { get; set; } // Filtro por faixa de preço (máximo)
        public double? MinArea { get; set; } // Filtro por faixa de área (mínimo)
        public double? MaxArea { get; set; } // Filtro por faixa de área (máximo)
        public string? Type { get; set; } // Filtro por tipo de imóvel
        public string? Purpose { get; set; } // Filtro por finalidade
    }
}
