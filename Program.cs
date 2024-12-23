using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;


List<Pessoa> hospedes = new List<Pessoa>();
List<Suite> suites = new List<Suite>();
List<Reserva> reservas = new List<Reserva>();

bool continuar = true;

while (continuar)
{
    Console.Clear();
    Console.WriteLine("Menu:");
    Console.WriteLine("1 - Cadastrar Hóspedes");
    Console.WriteLine("2 - Cadastrar Suítes");
    Console.WriteLine("3 - Cadastrar Hospedagem");
    Console.WriteLine("4 - Calcular e Listar");
    Console.WriteLine("5 - Sair");
    Console.Write("Escolha uma opção: ");
    int opcao = int.Parse(Console.ReadLine());

    switch (opcao)
    {
        case 1:
            // Cadastrar Hóspedes
            string nomeHospede;
            do
            {
                Console.Write("Digite o nome do hóspede: ");
                nomeHospede = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(nomeHospede))
                {
                    Console.WriteLine("Por favor, digite um nome válido.");
                }
            } while (string.IsNullOrEmpty(nomeHospede));

            Pessoa hospede = new Pessoa(nome: nomeHospede.ToUpper());
            hospedes.Add(hospede);
            Console.WriteLine("Hóspede cadastrado com sucesso!");
            break;
        case 2:
            // Cadastrar Suítes
            bool tipoSuiteValido = false;
            string tipoSuiteStr = "";

            while (!tipoSuiteValido)
            {
                Console.WriteLine("Tipos de suítes disponíveis:");
                Console.WriteLine("1 - Standard");
                Console.WriteLine("2 - Premium");
                Console.WriteLine("3 - Luxo");
                Console.WriteLine("4 - Presidencial");
                Console.Write("Escolha o tipo de suíte (1, 2, 3 ou 4): ");
                int tipoSuite = 0;

                // opção do tipo da suíte
                if (int.TryParse(Console.ReadLine(), out tipoSuite))
                {
                    tipoSuiteStr = tipoSuite switch
                    {
                        1 => "Standard",
                        2 => "Premium",
                        3 => "Luxo",
                        4 => "Presidencial",
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(tipoSuiteStr))
                    {
                        // existe uma suíte do tipo selecionado
                        if (suites.Any(suite => suite.TipoSuite == tipoSuiteStr))
                        {
                            Console.WriteLine($"A suíte {tipoSuiteStr} já está cadastrada. Escolha outro tipo.");
                        }
                        else
                        {
                            tipoSuiteValido = true; 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Tente novamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, insira um número entre 1 e 4.");
                }
            }

            int capacidade = 0;
            while (true)
            {
                // Lê a capacidade da suíte
                Console.Write("Digite a capacidade da suíte: ");
                if (int.TryParse(Console.ReadLine(), out capacidade) && capacidade > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Capacidade inválida. Por favor, insira um número positivo.");
                }
            }

            // Lê o valor da diária
            decimal valorDiaria = 0;
            while (true)
            {
                Console.Write("Digite o valor da diária: ");
                if (decimal.TryParse(Console.ReadLine(), out valorDiaria) && valorDiaria > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Valor inválido. Por favor, insira um valor positivo.");
                }
            }

            // Cadastra a nova suíte
            Suite suite = new Suite((suites.Count + 1), tipoSuite: tipoSuiteStr, capacidade: capacidade, valorDiaria: valorDiaria);
            suites.Add(suite);
            Console.WriteLine("Suíte cadastrada com sucesso!");
            break;
        case 3:
            // Cadastrar Hospedagem
            if (hospedes.Count == 0)
            {
                Console.WriteLine("Não há hóspedes cadastrados.");
                break;
            }

            if (suites.Count == 0)
            {
                Console.WriteLine("Não há suítes cadastradas.");
                break;
            }

            Console.WriteLine("Escolha um hóspede:");
            for (int i = 0; i < hospedes.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {hospedes[i].NomeCompleto}");
            }

            int indiceHospede = -1;
            bool inputValido = false;
            while (!inputValido)
            {
                Console.Write("Escolha o número do hóspede: ");
                string input = Console.ReadLine();

                // Verifica se a entrada é um número
                if (int.TryParse(input, out indiceHospede) && indiceHospede >= 1 && indiceHospede <= hospedes.Count)
                {
                    indiceHospede--; 
                    inputValido = true;
                }
                else
                {
                    Console.WriteLine("Opção inválida.");
                }
            }

            // Verifica se o hóspede já tem uma reserva
            Reserva reservaExistente = reservas.FirstOrDefault(r => r.Hospedes.Contains(hospedes[indiceHospede]));
            if (reservaExistente != null)
            {
                Console.WriteLine("Este hóspede já tem uma reserva.");
                break;
            }
            else
            {
                // Novo hóspede, nova reserva
                Console.WriteLine("Escolha uma suíte:");
                for (int i = 0; i < suites.Count; i++)
                {
                    Console.WriteLine($"{suites[i].Codigo} - {suites[i].TipoSuite} (Capacidade: {suites[i].Capacidade}, Valor Diária: {suites[i].ValorDiaria})");
                }

                int indiceSuite = -1;
                inputValido = false;
                while (!inputValido)
                {
                    Console.Write("Escolha o número da suíte: ");
                    string input = Console.ReadLine();

                    // Verifica se a entrada é um número válido
                    if (int.TryParse(input, out indiceSuite) && indiceSuite >= 1 && indiceSuite <= suites.Count)
                    {
                        indiceSuite--;
                        inputValido = true;
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Por favor, insira um número válido.");
                    }
                }

                // Verifica se a capacidade da suíte
                int quantidadeHospedesNaSuite = reservas.Where(r => r.Suite == suites[indiceSuite])
                                                         .Sum(r => r.Hospedes.Count);

                if (quantidadeHospedesNaSuite >= suites[indiceSuite].Capacidade)
                {
                    Console.WriteLine($"A capacidade da suíte é de {suites[indiceSuite].Capacidade} hóspedes.");
                    break;
                }

                // não pedir mais o numero de dias para outro hospede na mesma suite
                int diasReservados = 0;
                if (quantidadeHospedesNaSuite == 0)
                {
                    Console.Write("Digite o número de dias da reserva: ");
                    diasReservados = int.Parse(Console.ReadLine());
                }

                Reserva reserva = new Reserva(diasReservados: diasReservados);
                reserva.CadastrarSuite(suites[indiceSuite]);
                reserva.CadastrarHospedes(new List<Pessoa> { hospedes[indiceHospede] });

                reservas.Add(reserva);
                Console.WriteLine("Hospedagem cadastrada com sucesso!");
                break;
            }


        case 4:
            // Calcular e Listar
            if (reservas.Count == 0)
            {
                Console.WriteLine("Não há reservas cadastradas.");
                break;
            }

            Console.WriteLine("Relatório de Reservas:");

            // Agrupar as reservas por tipo de suíte
            var reservasPorSuite = reservas.GroupBy(r => r.Suite);

            foreach (var grupoSuite in reservasPorSuite)
            {
                // Para cada grupo de reserva, pegar a suíte e os hóspedes
                var suit = grupoSuite.Key;
                decimal valorTotal = grupoSuite.Sum(r => r.CalcularValorDiaria()); // Soma o valor de todas as reservas dessa suíte

                Console.WriteLine($"Suíte: {suit.Codigo} - {suit.TipoSuite}, Valor Total: {valorTotal:C}");

                // Listar os hóspedes da suíte
                Console.WriteLine("Hóspedes:");
                foreach (var reser in grupoSuite)
                {
                    foreach (var hosp in reser.Hospedes)
                    {
                        Console.WriteLine($"- {hosp.NomeCompleto}");
                    }
                }
            }
            break;

        case 5:
            continuar = false;
            Console.WriteLine("Saindo...");
            break;

        default:
            Console.WriteLine("Opção inválida, tente novamente.");
            break;
    }

    if (continuar)
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}


//// Cria os modelos de hóspedes e cadastra na lista de hóspedes
//List<Pessoa> hospedes = new List<Pessoa>();

//Pessoa p1 = new Pessoa(nome: "Hóspede 1");
//Pessoa p2 = new Pessoa(nome: "Hóspede 2");

//hospedes.Add(p1);
//hospedes.Add(p2);

//// Cria a suíte
//Suite suite = new Suite(tipoSuite: "Premium", capacidade: 2, valorDiaria: 30);

//// Cria uma nova reserva, passando a suíte e os hóspedes
//Reserva reserva = new Reserva(diasReservados: 5);
//reserva.CadastrarSuite(suite);
//reserva.CadastrarHospedes(hospedes);

//// Exibe a quantidade de hóspedes e o valor da diária
//Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
//Console.WriteLine($"Valor diária: {reserva.CalcularValorDiaria()}");