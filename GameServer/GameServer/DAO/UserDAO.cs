using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        //验证用户是否存在
        public User VerifyUser(MySqlConnection conn, string name, string password)
        {
            MySqlDataReader read = null;
            try
            {                
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password", conn);
                cmd.Parameters.AddWithValue("username", name);
                cmd.Parameters.AddWithValue("password", password);
                read = cmd.ExecuteReader();
                if (read.Read())
                {
                    int id = read.GetInt32("id");
                    Console.WriteLine(read.GetString("username"));
                    User user = new User(id, name, password);
                    return user;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("VerifyUser error"+e);
            }
            finally
            {
                if(read != null)
                {
                    read.Close();
                }
            }                      
            return null;
        }

        //添加用户
        public void AddUser(MySqlConnection conn, string username, string password)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username, password = @password",conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();
                //TODO 输出到日志

            }
            catch (Exception e)
            {
                Console.WriteLine("添加用户失败。"+e);
            }
        }

        public bool FindUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username",conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("查找失败" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }
    }
}
