using GraphQL.Types;
using System.IO;

namespace JssPlayground.GraphQL.GraphTypes
{
	public class FileGraphType : ObjectGraphType<FileSystemInfo>
	{
		public FileGraphType()
		{
			Name = "File";

			Field<StringGraphType>("name", "Filename of the file or directory including extension, if applicable", resolve: ResolveName);
			Field<StringGraphType>("extension", "Extension of the file, if applicable", resolve: ResolveExtension);
			Field<StringGraphType>("path", "Full path of the file or directory", resolve: ResolvePath);
			Field<EnumerationGraphType<FileSystemInfoType>>("type", "Whether this is a file or directory", resolve: ResolveType);
			Field<DateTimeGraphType>("created", "Created date and time of the file or directory", resolve: ResolveCreated);
			Field<DateTimeGraphType>("updated", "Updated date and time of the file or directory", resolve: ResolveUpdated);
			Field<DateTimeGraphType>("accessed", "Last accessed date and time of the file or directory", resolve: ResolveAccessed);

			Field<ListGraphType<FileGraphType>>("children", "Files and folders in the directory, if applicable", resolve: ResolveChildren);
		}

		private object ResolveType(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source is DirectoryInfo ? FileSystemInfoType.Directory : FileSystemInfoType.File;
		}

		private object ResolveChildren(ResolveFieldContext<FileSystemInfo> arg)
		{
			if (arg.Source is DirectoryInfo directory)
			{
				return directory.GetFileSystemInfos();
			}

			return null;
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

	public enum FileSystemInfoType
	{
		File,
		Directory
	}
}
