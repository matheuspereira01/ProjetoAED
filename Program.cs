using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBem-vindo ao Daily Control! \nAqui você registrar e visualizar seus gastos");
            List<Produto> produtosExistentes = new List<Produto>();
            Console.Write("\nInsira o seu nome: ");
            string nomeUsuario = Console.ReadLine();
            Console.Write("Insira o saldo da sua conta: ");
            float saldo = float.Parse(Console.ReadLine());
            Console.Write("Escreva o mês que deseja controlar os gastos: ");
            string mes = Console.ReadLine();
            Conta novaConta = new Conta(nomeUsuario, saldo, mes);
            Console.WriteLine("\nPronto, " + nomeUsuario + ", sua conta foi criada com sucesso! \nPressione qualquer tecla para continuar");
            Console.ReadKey();
                
            while (true)
            {
                Console.Write("\nDigite o dia da sua compra: ");
                int diaCompra = int.Parse(Console.ReadLine());
                Console.Write("Digite o nome do produto: ");
                string nomeProd = Console.ReadLine();
                Console.Write("Quantidade: ");
                int qntProduto = int.Parse(Console.ReadLine());
                Produto produtoEncontrado = SearchProduct(nomeProd);
                if (produtoEncontrado == null)
                {
                    Console.Write("Digite o valor unitário: ");
                    float valor = float.Parse(Console.ReadLine());
                    produtoEncontrado = new Produto(nomeProd, valor, diaCompra, qntProduto);
                    produtosExistentes.Add(produtoEncontrado);
                }

                Console.WriteLine("\nProduto adicionado na sua lista de gastos");
                novaConta.AdicionarGasto(produtoEncontrado, qntProduto);
         
            Voltar:
                Console.WriteLine("\nDeseja continuar adicionando novos produtos? [S/N]\n");
                string decisao = Console.ReadLine().ToUpper();
                if (decisao == "S")
                    continue;
                else if (decisao == "N")
                    break;
                else
                {
                    goto Voltar;
                }
            }
            EscreverRelatorio();
            Console.WriteLine("\nO relatório foi escrito com sucesso! ");
            Console.ReadKey();
            Produto SearchProduct(string product)
            {
                foreach (Produto produto in produtosExistentes)
                {
                    if (produto.getName() == product)
                        return produto;
                }
                return null;
            }
            void EscreverRelatorio()
            {
                List<Produto> produtosComprados = novaConta.getListProds();
                List<int> produtosValues = novaConta.getValueProds();
                string path = "C:\\Users\\User\\Desktop\\API\\" + novaConta.getName() + ".txt";
                using (StreamWriter sw = new StreamWriter(path, append: true))
                {
                    sw.Write("-------------[Compra do Mês de " + novaConta.getMes() + "]-------------\n");
                    sw.Write("Dono da conta: " + novaConta.getName());
                    sw.Write("\nSaldo inicial: " + novaConta.getSaldo());

                    float[] diascomprados = new float[31];

                    float somavalor = 0;
                    for (int i = 0; i < produtosComprados.Count; i++)
                    {
                        Produto produto = produtosComprados[i];
                        int diacompra = produto.getDia();
                        diascomprados[diacompra - 1] += produto.multValor();

                        int value = produtosValues[i];
                        sw.Write("\n\nComprado no dia " + produto.getDia() + "\nProduto:" + produto.getName() + "\nPreço:R$" + produto.getValor() + ",00 Quantidade:" + value + "\nTotal:R$" + produto.multValor() + ",00");
                        somavalor += value * produto.getValor();
                    }

                    sw.Write("\n\nGastos totais por dia:");

                    for (int i = 0; i < diascomprados.Length; i++)
                    {
                        if (diascomprados[i] > 0)
                        {
                            sw.Write("\nGastos do dia " + (i + 1) + " : R$" + diascomprados[i] + ",00");
                        }
                    }
                    Console.ReadKey();


                    sw.Write("\n\nGasto total: R$" + (somavalor) + ",00");
                    sw.Write("\nSaldo atual: R$" + (novaConta.getSaldo() - somavalor) + ",00\n\n");
                }
            }
        }
    }

    class Conta
    {
        private string nome;
        private float saldo;
        private string mes;
        List<Produto> produtosComprados = new List<Produto>();
        List<int> produtosValues = new List<int>();
        public Conta(string accountName, float accountSaldo, string accountMount)
        {
            this.nome = accountName;
            this.saldo = accountSaldo;
            this.mes = accountMount;
        }
        public string getName()
        {
            return nome;
        }
        public float getSaldo()
        {
            return saldo;
        }
        public string getMes()
        {
            return mes;
        }
        public void mostrarSaldo(float value)
        {
            this.saldo += value;
        }
        public List<Produto> getListProds()
        {
            return this.produtosComprados;
        }
        public List<int> getValueProds()
        {
            return this.produtosValues;
        }
        public void AdicionarGasto(Produto produto, int qtd)
        {
            produtosComprados.Add(produto);
            produtosValues.Add(qtd);

        }
    }
    class Produto
    {
        private string nome;
        private float valor;
        private int dia;
        private int quantidade;

        public Produto(string nomeProduto, float valorProduto, int diaProduto, int qntProduto)
        {
            this.nome = nomeProduto;
            this.valor = valorProduto;
            this.dia = diaProduto;
            this.quantidade = qntProduto;
        }
        public string getName()
        {
            return nome;
        }
        public float getValor()
        {
            return valor;
        }

        public int getDia()
        {
            return dia;
        }
        public int getQuant()
        {
            return quantidade;
        }

        public float multValor()
        {
            return quantidade * valor;
        }
    }
}