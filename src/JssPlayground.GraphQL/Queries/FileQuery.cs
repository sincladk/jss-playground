using GraphQL.Types;
using JssPlayground.GraphQL.GraphTypes;
using Sitecore;
using Sitecore.Data;
using Sitecore.Services.GraphQL.Content;
using Sitecore.Services.GraphQL.Schemas;
using System;
using System.IO;
using System.Web;

namespace JssPlayground.GraphQL.Queries
{
	public class FileQuery : RootFieldType<FileGraphType, FileSystemInfo>, IContentSchemaRootFieldType
	{
		public FileQuery() : base("file", "Supports querying for files in the filesystem")
		{
			base.Arguments = new QueryArguments
			{
				new QueryArgument<StringGraphType>
				{
					Name = "path",
					Description = "The path of the file system item you want to query. Leave off the starting slash (/) to be relative to the site root or prepend a slash (/) to be absolute to the drive"
				}
			};
		}

		public Database Database { get; set; }

		protected override FileSystemInfo Resolve(ResolveFieldContext context)
		{
			string path = context.GetArgument<string>("path");

			if (path == null)
			{
				throw new ArgumentNullException(nameof(path), "Must provide a path");
			}

			if (!path.StartsWith("/", StringComparison.Ordinal) && HttpContext.Current?.Server != null)
			{
				path = HttpContext.Current?.Server.MapPath(StringUtil.EnsurePrefix('/', path));
			}

			FileSystemInfo result = null;
			if (Directory.Exists(path))
			{
				result = new DirectoryInfo(path);
			}
			else if (File.Exists(path))
			{
				result = new FileInfo(path);
			}

			return result;
		}
	}
}
