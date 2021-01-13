using System.Linq;
using GitVersion.Helpers;

namespace GitVersion
{
    internal class Remote : IRemote
    {
        private static readonly LambdaEqualityHelper<IRemote> equalityHelper =
            new LambdaEqualityHelper<IRemote>(x => x.Name);
        private static readonly LambdaKeyComparer<IRemote, string> comparerHelper =
            new LambdaKeyComparer<IRemote, string>(x => x.Name);

        private readonly LibGit2Sharp.Remote innerRemote;

        internal Remote(LibGit2Sharp.Remote remote)
        {
            innerRemote = remote;
        }

        protected Remote()
        {
        }

        public int CompareTo(IRemote other) => comparerHelper.Compare(this, other);
        public override bool Equals(object obj) => Equals(obj as IRemote);
        public bool Equals(IRemote other) => equalityHelper.Equals(this, other);
        public override int GetHashCode() => equalityHelper.GetHashCode(this);
        public virtual string Name => innerRemote.Name;
        public virtual string RefSpecs => string.Join(", ", innerRemote.FetchRefSpecs.Select(r => r.Specification));
    }
}
