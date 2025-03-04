using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MauiAppMinhasCompras.Models; //Adiciona os Modelos que estão na pasta MauiAppMinhasCompras/Models que no caso desse arquivo, ele está usando o Produto.cs.
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn; //Propriedade que irá armazenar a "conexão" com o SQLite.

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait(); //Esse comando serve para criar uma tabela no SQLite caso ele não exista. O comando .Wait significa que ele esperará que a taréfa seja concluida.

        }

        //Funcionalidade de inserir produto no SQLite.
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        //Funcionalidade de atualizar um item da tabela produto do SQLite.
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade?, Preco=? WHERE id=?";
            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.id
                );
        }

        //Funcionalidade de delietar um item da tabela produto do SQLite.
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.id == id);
        }

        //Funcionalidade de listar todos os produtos na tabela do SQLite.
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        //Funcionalidade para procurar produtos na tabela do SQLite.
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * Produto WHERE descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        } 
    }
}
