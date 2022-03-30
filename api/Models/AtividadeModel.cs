namespace minimal_api_todo.Models
{
    public class AtividadeModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Status { get; set; }
        public bool Ativo{ get; set; }

    }
}
