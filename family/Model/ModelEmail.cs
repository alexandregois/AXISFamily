using System;
using System.Text.RegularExpressions;

namespace family.Model
{
	public class ModelEmail
	{
		public Boolean IsEmailValido(String paramEmail)
		{
			String expression = "^[\\w\\.-]+@([\\w\\-]+\\.)+[A-Z]{2,4}$";

			Regex rg = new Regex(expression);

			return rg.IsMatch(paramEmail);
		}
	}
}
