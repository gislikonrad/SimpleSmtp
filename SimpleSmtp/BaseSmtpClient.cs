using SimpleSmtp.Implementations;
using SimpleSmtp.Interfaces;
using SimpleSmtp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSmtp
{
	public abstract class BaseSmtpClient
	{		
		private const string MAIL_FROM_COMMAND_FORMAT = "MAIL FROM: {0}";
		private const string RCPT_TO_COMMAND_FORMAT = "RCPT TO: {0}";
		private const string HELO_COMMAND_FORMAT = "HELO {0}";
		private const string EHLO_COMMAND_FORMAT = "EHLO {0}";
		private const string DATA_COMMAND = "DATA";
		private const string DATA_END = "\r\n.\r\n";
		private const string QUIT_COMMAND = "QUIT";

		public const int SMTP_DEFAULT_PORT = 25;
		public const int SSMTP_DEFAULT_PORT = 465;

		protected IConnectionManager ConnectionManager;

		public BaseSmtpClient(string host)
			: this(host, SMTP_DEFAULT_PORT)
		{

		}

		public BaseSmtpClient(string host, bool useSsl)
			: this(host, useSsl ? SSMTP_DEFAULT_PORT : SMTP_DEFAULT_PORT, useSsl)
		{
		}

		public BaseSmtpClient(string host, int port)
			: this(host, port, false)
		{
		}

		public BaseSmtpClient(string host, int port, bool useSsl)
		{
			this.ConnectionManager = new TcpClientConnectionManager();
			this.ConnectionManager.ConnectAsync(host, port, useSsl).Wait();
		}

		protected virtual async Task<SmtpResponse> HeloBaseAsync()
		{
			return await this.ConnectionManager.ExecuteCommandAsync(string.Format(HELO_COMMAND_FORMAT, GetDnsHostName()));
		}

		protected virtual async Task<SmtpResponse> EhloBaseAsync()
		{
			return await this.ConnectionManager.ExecuteCommandAsync(string.Format(EHLO_COMMAND_FORMAT, GetDnsHostName()));
		}

		protected virtual async Task<SmtpResponse> MailFromBaseAsync(string from)
		{
			return await this.ConnectionManager.ExecuteCommandAsync(string.Format(MAIL_FROM_COMMAND_FORMAT, from));
		}

		protected virtual async Task<SmtpResponse> RcptToBaseAsync(string to)
		{
			return await this.ConnectionManager.ExecuteCommandAsync(string.Format(RCPT_TO_COMMAND_FORMAT, to));
		}

		protected virtual async Task<SmtpResponse> DataBaseAsync(string data)
		{
			await this.ConnectionManager.ExecuteCommandAsync(DATA_COMMAND);
			return await this.ConnectionManager.ExecuteCommandAsync(data + DATA_END);
		}

		protected virtual string GetDnsHostName()
		{
			return Dns.GetHostEntry(Dns.GetHostName()).HostName;
		}

		public void Dispose()
		{
			var task = this.ConnectionManager.ExecuteCommandAsync(QUIT_COMMAND);
			task.Wait();
			this.ConnectionManager.Dispose();
		}
	}
}
