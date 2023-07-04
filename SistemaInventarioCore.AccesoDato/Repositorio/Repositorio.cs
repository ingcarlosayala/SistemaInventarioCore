using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.Models.Especificaciones;
using SistemaInventarioCore.AccesoDato.Data;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.AccesoDato.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string IncluirPropiedad = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);

            }
            else if (IncluirPropiedad != null)
            {
                foreach (var item in IncluirPropiedad.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string IncluirPropiedad = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);

            }else if (IncluirPropiedad != null)
            {
                foreach (var item in IncluirPropiedad.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }

            }else if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }


        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);   //  select /* from where ....
            }
            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);    //  ejemplo "Categoria,Marca"
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }
    }
}
