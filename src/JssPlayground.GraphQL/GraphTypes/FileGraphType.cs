using GraphQL.Types;
using System.IO;

namespace JssPlayground.GraphQL.GraphTypes
{
	public class FileGraphType : ObjectGraphType<FileSystemInfo>
	{
		public FileGraphType()
		{
			Name = "File";

			Field<StringGraphType>("name", "Filename of the file not including extension", resolve: ResolveName);
			Field<StringGraphType>("extension", "Extension of the file", resolve: ResolveExtension);
			Field<StringGraphType>("path", "Full path of the file", resolve: ResolvePath);
			Field<DateTimeGraphType>("created", "Created date and time of the file", resolve: ResolveCreated);
			Field<DateTimeGraphType>("updated", "Updated date and time of the file", resolve: ResolveUpdated);
			Field<DateTimeGraphType>("accessed", "Last accessed date and time of the file", resolve: ResolveAccessed);
		}

		private object ResolveAccessed(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.LastAccessTime;
		}

		private object ResolveUpdated(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.LastWriteTime;
		}

		private object ResolveCreated(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.CreationTime;
		}

		private object ResolvePath(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.FullName;
		}

		private object ResolveExtension(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.Extension;
		}

		private object ResolveName(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source.Name;
		}
	}
}
