using Supermercado.Domain.Entities;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Infrastructure.Data;

public static class SupermercadoDbSeeder
{
    public static async Task SeedAsync(SupermercadoDbContext context)
    {
        var currentDate = DateTime.UtcNow;

        if (!context.Categories.Any())
        {
            var categorias = new List<Category>
            {
                new Category("Padaria", "Pães, bolos e salgados"),
                new Category("Açougue", "Carnes bovinas, suínas e aves"),
                new Category("Frios e Laticínios", "Queijos, presuntos, leites e iogurtes"),
                new Category("Limpeza", "Produtos de limpeza geral"),
                new Category("Hortifruti", "Frutas, legumes e verduras frescos")
            };

            await context.Categories.AddRangeAsync(categorias);
            await context.SaveChangesAsync();
        }

        if (!context.Products.Any())
        {
            var categorias = context.Categories.ToList();
            var padaria = categorias.First(c => c.Name == "Padaria").Id;
            var acougue = categorias.First(c => c.Name == "Açougue").Id;
            var frios = categorias.First(c => c.Name == "Frios e Laticínios").Id;
            var limpeza = categorias.First(c => c.Name == "Limpeza").Id;
            var hortifruti = categorias.First(c => c.Name == "Hortifruti").Id;

            var random = new Random(42); // Para reprodutibilidade
            var products = new List<Product>();

            // Função auxiliar para criar produto
            Product CriarProduto(string nome, string desc, string ean, decimal preco, Guid catId, int diasValidade, int estoque)
            {
                var barcode = new Barcode(ean);
                var money = new Money(preco);
                var validade = currentDate.AddDays(diasValidade);
                
                var produto = new Product(nome, desc, barcode, money, catId, validade, 40m, currentDate);
                produto.AddStock(estoque);
                return produto;
            }

            // Padaria
            products.Add(CriarProduto("Pão Francês 1kg", "Pão francês fresquinho", "7891000000010", 14.90m, padaria, 6, 50));
            products.Add(CriarProduto("Pão de Forma Tradicional", "Pão de forma fatiado 500g", "7891000000027", 8.50m, padaria, 10, 30));
            products.Add(CriarProduto("Pão de Queijo Congelado", "Pacote 1kg", "7891000000034", 22.90m, padaria, 60, 20));
            products.Add(CriarProduto("Bolo de Chocolate", "Bolo caseiro 500g", "7891000000041", 18.00m, padaria, 7, 15));
            products.Add(CriarProduto("Bolo de Cenoura", "Bolo caseiro 500g", "7891000000058", 16.00m, padaria, 7, 10));
            products.Add(CriarProduto("Baguete Tradicional", "Baguete média", "7891000000065", 6.00m, padaria, 6, 25));
            products.Add(CriarProduto("Croissant de Manteiga", "Croissant 100g", "7891000000072", 5.50m, padaria, 6, 40));
            products.Add(CriarProduto("Sonho com Creme", "Sonho tradicional", "7891000000089", 4.00m, padaria, 6, 30));
            products.Add(CriarProduto("Torta de Morango", "Torta fatia", "7891000000096", 9.90m, padaria, 6, 12));
            products.Add(CriarProduto("Pão Sírio", "Pacote 300g", "7891000000102", 7.50m, padaria, 15, 20));
            products.Add(CriarProduto("Broa de Milho", "Broa artesanal", "7891000000119", 3.50m, padaria, 8, 30));
            products.Add(CriarProduto("Pão Integral", "Pão de forma integral", "7891000000126", 10.50m, padaria, 12, 25));

            // Açougue
            products.Add(CriarProduto("Picanha Bovina 1kg", "Picanha a vácuo", "7892000000019", 79.90m, acougue, 30, 20));
            products.Add(CriarProduto("Alcatra Bovina 1kg", "Peça de alcatra limpa", "7892000000026", 45.90m, acougue, 25, 30));
            products.Add(CriarProduto("Coxão Mole 1kg", "Carne para panela", "7892000000033", 38.90m, acougue, 20, 25));
            products.Add(CriarProduto("Filé de Peito de Frango", "Bandeja 1kg", "7892000000040", 21.90m, acougue, 15, 50));
            products.Add(CriarProduto("Coxinha da Asa", "Bandeja 1kg", "7892000000057", 18.90m, acougue, 15, 40));
            products.Add(CriarProduto("Linguiça Toscana", "Pacote 1kg", "7892000000064", 24.90m, acougue, 30, 35));
            products.Add(CriarProduto("Pernil Suíno com Osso", "Peça 1kg", "7892000000071", 22.90m, acougue, 20, 15));
            products.Add(CriarProduto("Costela Bovina", "Peça 1kg", "7892000000088", 26.90m, acougue, 25, 20));
            products.Add(CriarProduto("Bacon em Cubos", "Pacote 200g", "7892000000095", 14.50m, acougue, 60, 40));
            products.Add(CriarProduto("Carne Moída Patinho", "Bandeja 500g", "7892000000101", 23.90m, acougue, 12, 30));
            products.Add(CriarProduto("Coração de Frango", "Bandeja 500g", "7892000000118", 15.90m, acougue, 12, 25));
            products.Add(CriarProduto("Costelinha Suína", "Peça 1kg", "7892000000125", 28.90m, acougue, 20, 18));

            // Frios e Laticínios
            products.Add(CriarProduto("Queijo Mussarela", "Fatiado 1kg", "7893000000018", 45.00m, frios, 40, 30));
            products.Add(CriarProduto("Presunto Cozido", "Fatiado 1kg", "7893000000025", 28.00m, frios, 30, 40));
            products.Add(CriarProduto("Leite Integral", "Caixa 1L", "7893000000032", 5.50m, frios, 120, 200));
            products.Add(CriarProduto("Leite Desnatado", "Caixa 1L", "7893000000049", 5.50m, frios, 120, 100));
            products.Add(CriarProduto("Manteiga com Sal", "Pote 200g", "7893000000056", 12.90m, frios, 90, 50));
            products.Add(CriarProduto("Margarina Tradicional", "Pote 500g", "7893000000063", 8.90m, frios, 180, 60));
            products.Add(CriarProduto("Iogurte de Morango", "Garrafa 1L", "7893000000070", 11.50m, frios, 30, 40));
            products.Add(CriarProduto("Iogurte Natural", "Copo 170g", "7893000000087", 3.20m, frios, 25, 60));
            products.Add(CriarProduto("Requeijão Tradicional", "Copo 200g", "7893000000094", 8.50m, frios, 60, 45));
            products.Add(CriarProduto("Queijo Prato", "Fatiado 1kg", "7893000000100", 47.00m, frios, 40, 25));
            products.Add(CriarProduto("Queijo Parmesão Ralado", "Pacote 50g", "7893000000117", 6.50m, frios, 180, 80));
            products.Add(CriarProduto("Creme de Leite", "Caixinha 200g", "7893000000124", 4.20m, frios, 180, 150));

            // Limpeza
            products.Add(CriarProduto("Detergente Líquido", "Neutro 500ml", "7894000000017", 2.50m, limpeza, 730, 200));
            products.Add(CriarProduto("Sabão em Pó", "Caixa 1kg", "7894000000024", 14.90m, limpeza, 730, 100));
            products.Add(CriarProduto("Amaciante Tradicional", "Garrafa 2L", "7894000000031", 12.50m, limpeza, 730, 80));
            products.Add(CriarProduto("Água Sanitária", "Garrafa 1L", "7894000000048", 3.80m, limpeza, 360, 120));
            products.Add(CriarProduto("Desinfetante Pinho", "Garrafa 500ml", "7894000000055", 4.50m, limpeza, 730, 90));
            products.Add(CriarProduto("Esponja Multiuso", "Pacote com 4", "7894000000062", 6.00m, limpeza, 1800, 150));
            products.Add(CriarProduto("Limpador Multiuso", "Frasco 500ml", "7894000000079", 5.50m, limpeza, 730, 100));
            products.Add(CriarProduto("Álcool Líquido 70%", "Garrafa 1L", "7894000000086", 8.90m, limpeza, 360, 80));
            products.Add(CriarProduto("Sabão em Pedra", "Pacote 5 unidades", "7894000000093", 10.50m, limpeza, 730, 60));
            products.Add(CriarProduto("Saco de Lixo 50L", "Rolo com 30 un", "7894000000109", 15.00m, limpeza, 3600, 70));
            products.Add(CriarProduto("Saco de Lixo 100L", "Rolo com 15 un", "7894000000116", 18.00m, limpeza, 3600, 50));
            products.Add(CriarProduto("Limpa Vidros", "Frasco 500ml", "7894000000123", 7.90m, limpeza, 730, 40));

            // Hortifruti
            products.Add(CriarProduto("Banana Nanica 1kg", "Banana madura", "7895000000016", 5.90m, hortifruti, 7, 50));
            products.Add(CriarProduto("Maçã Fuji 1kg", "Maçã selecionada", "7895000000023", 9.90m, hortifruti, 15, 40));
            products.Add(CriarProduto("Laranja Pera 1kg", "Ideal para suco", "7895000000030", 4.50m, hortifruti, 20, 100));
            products.Add(CriarProduto("Tomate Carmem 1kg", "Tomate para salada", "7895000000047", 8.90m, hortifruti, 10, 60));
            products.Add(CriarProduto("Cebola Branca 1kg", "Cebola graúda", "7895000000054", 6.50m, hortifruti, 30, 80));
            products.Add(CriarProduto("Alho 100g", "Cabeça de alho", "7895000000061", 3.00m, hortifruti, 60, 100));
            products.Add(CriarProduto("Batata Inglesa 1kg", "Batata lavada", "7895000000078", 7.90m, hortifruti, 20, 120));
            products.Add(CriarProduto("Cenoura 1kg", "Cenoura fresca", "7895000000085", 5.50m, hortifruti, 15, 50));
            products.Add(CriarProduto("Limão Tahiti 1kg", "Limão suculento", "7895000000092", 4.90m, hortifruti, 20, 40));
            products.Add(CriarProduto("Alface Crespa", "Unidade", "7895000000108", 2.50m, hortifruti, 6, 30));
            products.Add(CriarProduto("Couve Manteiga", "Maço", "7895000000115", 3.00m, hortifruti, 6, 25));
            products.Add(CriarProduto("Uva Thompson 500g", "Uva sem semente", "7895000000122", 12.90m, hortifruti, 10, 30));

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}
