using System;
using GraphQL.Types;
using System.IO;

namespace JssPlayground.GraphQL.GraphTypes
{
	public class FileGraphType : ObjectGraphType<FileSystemInfo>
	{
		protected const string CHILDREN_NAME_FILTER_ARGUMENT_NAME = "nameFilter";
		protected const string CHILDREN_TYPE_FILTER_ARGUMENT_NAME = "typeFilter";

		public FileGraphType()
		{
			Name = "File";

			Field<StringGraphType>("name", "Filename of the file or directory including extension, if applicable",
				resolve: ResolveName);
			Field<StringGraphType>("extension", "Extension of the file, if applicable", resolve: ResolveExtension);
			Field<StringGraphType>("path", "Full path of the file or directory", resolve: ResolvePath);
			Field<EnumerationGraphType<FileSystemInfoType>>("type", "Whether this is a file or directory",
				resolve: ResolveType);
			Field<DateTimeGraphType>("created", "Created date and time of the file or directory",
				resolve: ResolveCreated);
			Field<DateTimeGraphType>("updated", "Updated date and time of the file or directory",
				resolve: ResolveUpdated);
			Field<DateTimeGraphType>("accessed", "Last accessed date and time of the file or directory",
				resolve: ResolveAccessed);

			Field<ListGraphType<FileGraphType>>("children", "Files and folders in the directory, if applicable",
				resolve: ResolveChildren,
				arguments:
				new QueryArguments(
					new QueryArgument<StringGraphType>
					{
						Name = CHILDREN_NAME_FILTER_ARGUMENT_NAME,
						Description =
							"The search string to match against the names of the child directories and files. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions."
					}, new QueryArgument<EnumerationGraphType<FileSystemInfoType>>
					{
						Name = CHILDREN_TYPE_FILTER_ARGUMENT_NAME,
						Description = "Filter by files or directories only"
					}));
		}

		private object ResolveType(ResolveFieldContext<FileSystemInfo> arg)
		{
			return arg.Source is DirectoryInfo ? FileSystemInfoType.Directory : FileSystemInfoType.File;
		}

		private object ResolveChildren(ResolveFieldContext<FileSystemInfo> arg)
		{
			if (arg.Source is DirectoryInfo directory)
			{
				Func<DirectoryInfo, string, FileSystemInfo[]> getChildren = null;
				if (arg.HasArgument(CHILDREN_TYPE_FILTER_ARGUMENT_NAME))
				{
					FileSystemInfoType type = (FileSystemInfoType)arg.Arguments[CHILDREN_TYPE_FILTER_ARGUMENT_NAME];
					switch (type)
					{
						case FileSystemInfoType.File:
							getChildren = (di, name) => string.IsNullOrEmpty(name) ? di.GetFiles() : di.GetFiles(name);
							break;
						case FileSystemInfoType.Directory:
							getChildren = (di, name) => string.IsNullOrEmpty(name) ? di.GetDirectories() : di.GetDirectories(name);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					getChildren = (di, name) => string.IsNullOrEmpty(name) ? di.GetFileSystemInfos() : di.GetFileSystemInfos(name);
				}

				arg.Arguments.TryGetValue(CHILDREN_NAME_FILTER_ARGUMENT_NAME, out object nameFilter);
				return getChildren(directory, nameFilter?.ToString());
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