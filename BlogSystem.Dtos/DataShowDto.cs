using System;

namespace BlogSystem.Dtos
{
    public class DataShowDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Icons { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }
        public int Messages { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}