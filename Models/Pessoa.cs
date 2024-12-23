namespace DesafioProjetoHospedagem.Models;

public class Pessoa
{
    public Pessoa() { }

    public Pessoa(string nome)
    {
        // nome completo em partes
        var nomeParts = nome.Split(' ');
        // A primeira palavra vai para o 'Nome' e o restante vai para o 'Sobrenome'
        Nome = nomeParts[0];
        // Junta o restante das palavras como 'Sobrenome'
        Sobrenome = string.Join(" ", nomeParts.Skip(1)); 
    }

    public Pessoa(string nome, string sobrenome)
    {
        Nome = nome;
        Sobrenome = sobrenome;
    }

    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string NomeCompleto => $"{Nome} {Sobrenome}".ToUpper();
}