using System;
using System.Collections.Generic;
using System.Linq;

class Aluno
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    private List<Disciplina> disciplinasMatriculadas;

    public Aluno(int id, string nome)
    {
        Id = id;
        Nome = nome;
        disciplinasMatriculadas = new List<Disciplina>();
    }

    public bool PodeMatricular(Curso curso)
    {
        return disciplinasMatriculadas.Count < 6 && disciplinasMatriculadas.All(d => curso.Disciplinas.Contains(d));
    }

    public void AdicionarDisciplina(Disciplina disciplina)
    {
        if (!disciplinasMatriculadas.Contains(disciplina))
            disciplinasMatriculadas.Add(disciplina);
    }

    public void RemoverDisciplina(Disciplina disciplina)
    {
        disciplinasMatriculadas.Remove(disciplina);
    }

    public List<Disciplina> GetDisciplinas()
    {
        return disciplinasMatriculadas;
    }
}

class Disciplina
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    private List<Aluno> alunos;

    public Disciplina(int id, string descricao)
    {
        Id = id;
        Descricao = descricao;
        alunos = new List<Aluno>();
    }

    public bool MatricularAluno(Aluno aluno)
    {
        if (alunos.Count >= 15) return false;
        if (aluno.GetDisciplinas().Count >= 6) return false;

        if (!alunos.Contains(aluno))
        {
            alunos.Add(aluno);
            aluno.AdicionarDisciplina(this);
            return true;
        }
        return false;
    }

    public bool DesmatricularAluno(Aluno aluno)
    {
        if (alunos.Contains(aluno))
        {
            alunos.Remove(aluno);
            aluno.RemoverDisciplina(this);
            return true;
        }
        return false;
    }

    public List<Aluno> GetAlunos()
    {
        return alunos;
    }
}

class Curso
{
    public int Id { get; private set; }
    public string Descricao { get; private set; }
    public List<Disciplina> Disciplinas { get; private set; }

    public Curso(int id, string descricao)
    {
        Id = id;
        Descricao = descricao;
        Disciplinas = new List<Disciplina>();
    }

    public bool AdicionarDisciplina(Disciplina disciplina)
    {
        if (Disciplinas.Count >= 12) return false;
        if (!Disciplinas.Any(d => d.Id == disciplina.Id))
        {
            Disciplinas.Add(disciplina);
            return true;
        }
        return false;
    }

    public Disciplina PesquisarDisciplina(int id)
    {
        return Disciplinas.Find(d => d.Id == id);
    }

    public bool RemoverDisciplina(int id)
    {
        var d = PesquisarDisciplina(id);
        if (d != null && d.GetAlunos().Count == 0)
        {
            Disciplinas.Remove(d);
            return true;
        }
        return false;
    }
}

class Escola
{
    private List<Curso> cursos;

    public Escola()
    {
        cursos = new List<Curso>();
    }

    public bool AdicionarCurso(Curso curso)
    {
        if (cursos.Count >= 5) return false;
        if (!cursos.Any(c => c.Id == curso.Id))
        {
            cursos.Add(curso);
            return true;
        }
        return false;
    }

    public Curso PesquisarCurso(int id)
    {
        return cursos.Find(c => c.Id == id);
    }

    public bool RemoverCurso(int id)
    {
        var c = PesquisarCurso(id);
        if (c != null && c.Disciplinas.Count == 0)
        {
            cursos.Remove(c);
            return true;
        }
        return false;
    }

    public List<Curso> GetCursos()
    {
        return cursos;
    }
}

class Program
{
    static void Main()
    {
        Escola escola = new Escola();
        List<Aluno> alunos = new List<Aluno>();

        int opcao;
        do
        {
            Console.WriteLine("\nMENU:");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar curso");
            Console.WriteLine("2. Pesquisar curso");
            Console.WriteLine("3. Remover curso");
            Console.WriteLine("4. Adicionar disciplina no curso");
            Console.WriteLine("5. Pesquisar disciplina");
            Console.WriteLine("6. Remover disciplina do curso");
            Console.WriteLine("7. Matricular aluno na disciplina");
            Console.WriteLine("8. Remover aluno da disciplina");
            Console.WriteLine("9. Pesquisar aluno");
            Console.Write("Opção: ");
            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    Console.Write("Id do curso: ");
                    int idCurso = int.Parse(Console.ReadLine());
                    Console.Write("Descrição: ");
                    string descCurso = Console.ReadLine();
                    if (escola.AdicionarCurso(new Curso(idCurso, descCurso)))
                        Console.WriteLine("Curso adicionado!");
                    else
                        Console.WriteLine("Não foi possível adicionar (limite ou duplicado).");
                    break;

                case 2:
                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    var curso = escola.PesquisarCurso(idCurso);
                    if (curso != null)
                    {
                        Console.WriteLine($"Curso: {curso.Id} - {curso.Descricao}");
                        foreach (var d in curso.Disciplinas)
                            Console.WriteLine($"  Disciplina: {d.Id} - {d.Descricao}");
                    }
                    else Console.WriteLine("Curso não encontrado.");
                    break;

                case 3:
                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    if (escola.RemoverCurso(idCurso))
                        Console.WriteLine("Curso removido.");
                    else
                        Console.WriteLine("Não foi possível remover (pode haver disciplinas associadas).");
                    break;

                case 4:
                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    curso = escola.PesquisarCurso(idCurso);
                    if (curso != null)
                    {
                        Console.Write("Id da disciplina: ");
                        int idDisc = int.Parse(Console.ReadLine());
                        Console.Write("Descrição: ");
                        string descDisc = Console.ReadLine();
                        if (curso.AdicionarDisciplina(new Disciplina(idDisc, descDisc)))
                            Console.WriteLine("Disciplina adicionada!");
                        else
                            Console.WriteLine("Não foi possível adicionar (limite ou duplicado).");
                    }
                    else Console.WriteLine("Curso não encontrado.");
                    break;

                case 5:
                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    curso = escola.PesquisarCurso(idCurso);
                    if (curso != null)
                    {
                        Console.Write("Id da disciplina: ");
                        int idDisc = int.Parse(Console.ReadLine());
                        var disc = curso.PesquisarDisciplina(idDisc);
                        if (disc != null)
                        {
                            Console.WriteLine($"Disciplina: {disc.Id} - {disc.Descricao}");
                            foreach (var a in disc.GetAlunos())
                                Console.WriteLine($"  Aluno: {a.Id} - {a.Nome}");
                        }
                        else Console.WriteLine("Disciplina não encontrada.");
                    }
                    else Console.WriteLine("Curso não encontrado.");
                    break;

                case 6:
                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    curso = escola.PesquisarCurso(idCurso);
                    if (curso != null)
                    {
                        Console.Write("Id da disciplina: ");
                        int idDisc = int.Parse(Console.ReadLine());
                        if (curso.RemoverDisciplina(idDisc))
                            Console.WriteLine("Disciplina removida.");
                        else
                            Console.WriteLine("Não foi possível remover (pode haver alunos matriculados).");
                    }
                    else Console.WriteLine("Curso não encontrado.");
                    break;

                case 7:
                    Console.Write("Id do aluno: ");
                    int idAluno = int.Parse(Console.ReadLine());
                    var aluno = alunos.Find(a => a.Id == idAluno);
                    if (aluno == null)
                    {
                        Console.Write("Nome do aluno: ");
                        string nomeAluno = Console.ReadLine();
                        aluno = new Aluno(idAluno, nomeAluno);
                        alunos.Add(aluno);
                    }

                    Console.Write("Id do curso: ");
                    idCurso = int.Parse(Console.ReadLine());
                    curso = escola.PesquisarCurso(idCurso);
                    if (curso != null)
                    {
                        Console.Write("Id da disciplina: ");
                        int idDisc = int.Parse(Console.ReadLine());
                        var disc = curso.PesquisarDisciplina(idDisc);
                        if (disc != null)
                        {
                            if (disc.MatricularAluno(aluno))
                                Console.WriteLine("Aluno matriculado!");
                            else
                                Console.WriteLine("Não foi possível matricular.");
                        }
                        else Console.WriteLine("Disciplina não encontrada.");
                    }
                    else Console.WriteLine("Curso não encontrado.");
                    break;

                case 8:
                    Console.Write("Id do aluno: ");
                    idAluno = int.Parse(Console.ReadLine());
                    aluno = alunos.Find(a => a.Id == idAluno);
                    if (aluno != null)
                    {
                        Console.Write("Id do curso: ");
                        idCurso = int.Parse(Console.ReadLine());
                        curso = escola.PesquisarCurso(idCurso);
                        if (curso != null)
                        {
                            Console.Write("Id da disciplina: ");
                            int idDisc = int.Parse(Console.ReadLine());
                            var disc = curso.PesquisarDisciplina(idDisc);
                            if (disc != null && disc.DesmatricularAluno(aluno))
                                Console.WriteLine("Aluno removido da disciplina.");
                            else
                                Console.WriteLine("Não foi possível remover.");
                        }
                        else Console.WriteLine("Curso não encontrado.");
                    }
                    else Console.WriteLine("Aluno não encontrado.");
                    break;

                case 9:
                    Console.Write("Id do aluno: ");
                    idAluno = int.Parse(Console.ReadLine());
                    aluno = alunos.Find(a => a.Id == idAluno);
                    if (aluno != null)
                    {
                        Console.WriteLine($"Aluno: {aluno.Id} - {aluno.Nome}");
                        foreach (var d in aluno.GetDisciplinas())
                            Console.WriteLine($"  Disciplina: {d.Id} - {d.Descricao}");
                    }
                    else Console.WriteLine("Aluno não encontrado.");
                    break;
            }

        } while (opcao != 0);
    }
}
