using iWorkDemo.model;
using MySqlConnector;

namespace iWorkDemo.dbo
{
    public class EmployeeDBO
    {
        MySqlConnection connection;
        public MySqlConnection getConnection()
        {
            connection = new MySqlConnection("Server=localhost;User ID=nahom;Password=AlphaGeek23;Database=iWork");
            return connection;
        }
        public Boolean create(Employee employee)
        {
            MySqlConnection conn=getConnection();
            conn.Open();
            using var query = new MySqlCommand("Insert into employee (`firstName`,`lastName`,`email`,`gender`,`password`) values (@firstName,@lastName,@email,@gender,@password)",conn);
            query.Parameters.AddWithValue("@firstName",employee.firstName);
            query.Parameters.AddWithValue("@lastName", employee.lastName);
            query.Parameters.AddWithValue("@email", employee.email);
            query.Parameters.AddWithValue("@gender", employee.gender);
            query.Parameters.AddWithValue("@password",employee.password);
            query.ExecuteNonQuery();
            long autoId = query.LastInsertedId;
            return autoId >= 0;
        }
        public Employee? getById(long id)
        {
            MySqlConnection conn=getConnection();
            conn.Open();
            using var query = new MySqlCommand("select * from employee where id = @id",conn);
            query.Parameters.AddWithValue("id", id);
            List<Employee> list=parseQuery(query);
            return list.Count > 0 ? list[0] : null;
        }
        public Employee? getByEmail(string email)
        {
            MySqlConnection conn = getConnection();
            conn.Open();
            using var query = new MySqlCommand("select * from employee where email = @email", conn);
            query.Parameters.AddWithValue("email", email);
            List<Employee> list = parseQuery(query);
            return list.Count > 0 ? list[0] : null;
        }
        public List<Employee> getAll()
        {
            MySqlConnection conn=getConnection();
            conn.Open();
            using var query = new MySqlCommand("select * from employee", conn);
            return parseQuery(query);
        }
        private List<Employee> parseQuery(MySqlCommand query)
        {
            using var reader = query.ExecuteReader();
            List<Employee> employees = new List<Employee>();
            while (reader.Read())
            {
                Employee emp = new Employee();
                emp.id = (long)(int)reader.GetValue(0);
                emp.firstName = reader.GetString(1);
                emp.lastName = reader.GetString(2);
                emp.email = reader.GetString(3);
                emp.gender = reader.GetString(4);
                emp.password = reader.GetString(5);
                employees.Add(emp);
            }
            return employees;
        }

        internal void update(Employee employee)
        {
            MySqlConnection conn = getConnection();
            conn.Open();
            using var query = new MySqlCommand("update employee set firstName=@firstName, lastName=@lastName, email=@email, gender=@gender where id=@id", conn);
            query.Parameters.AddWithValue("@firstName", employee.firstName);
            query.Parameters.AddWithValue("@lastName", employee.lastName);
            query.Parameters.AddWithValue("@email", employee.email);
            query.Parameters.AddWithValue("@gender", employee.gender);
            query.Parameters.AddWithValue("@password", employee.password);
            query.Parameters.AddWithValue("@id", employee.id);
            query.ExecuteNonQuery();
        }

        internal void remove(Employee employee)
        {
            MySqlConnection conn = getConnection();
            conn.Open();
            using var query = new MySqlCommand("delete from employee where id=@id", conn);
           
            query.Parameters.AddWithValue("@id", employee.id);
            query.ExecuteNonQuery();
        }
    }
}
