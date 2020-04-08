using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractAlgebraSpace
{
    class AbstractAlgebra
    {
    }

    public interface IGroupoid<T>
    {
        T Operation(T a, T b);
    }

    public interface ISemigroup<T> : IGroupoid<T>
    {

    }

    public interface IMonoid<T> : ISemigroup<T>
    {
        T Identity { get; }
    }

    public interface IGroup<T> : IMonoid<T>
    {
        T Inverse(T t);
    }

    public interface IAbelianGroup<T> : IGroup<T>
    {

    }

    public interface IFiniteGroup<T> : IGroup<T>
    {
        T Closure(T a, T b);
    }
    public interface IRingoid<T, A, M>
        where A : IGroupoid<T>
        where M : IGroupoid<T>
    {
        A Addition { get; }
        M Multiplication { get; }

        T Distribute(T a, T b);
    }

    public interface ISemiring<T, A, M> : IRingoid<T, A, M>
        where A : ISemigroup<T>
        where M : ISemigroup<T>
    {

    }

    public interface IRing<T, A, M> : ISemiring<T, A, M>
        where A : IGroup<T>
        where M : ISemigroup<T>
    {

    }

    public interface IRingWithUnity<T, A, M> : IRing<T, A, M>
        where A : IGroup<T>
        where M : IMonoid<T>
    {

    }

    public interface IDivisionRing<T, A, M> : IRingWithUnity<T, A, M>
        where A : IGroup<T>
        where M : IGroup<T>
    {

    }

    public interface IField<T, A, M> : IDivisionRing<T, A, M>
        where A : IAbelianGroup<T>
        where M : IAbelianGroup<T>
    {

    }

    public interface IVectorField<T1, T2, T3, T4, T5, T6> : IField<T1, T4, T5>
        where T4 : IAbelianGroup<T1>
        where T5 : IAbelianGroup<T1>
    {
    }


    public interface IModule<
        TScalar,
        TVector,
        TScalarRing,
        TScalarAddativeGroup,
        TScalarMultiplicativeSemigroup,
        TVectorAddativeAbelianGroup
    >
        where TScalarRing : IRing<TScalar, TScalarAddativeGroup, TScalarMultiplicativeSemigroup>
        where TScalarAddativeGroup : IGroup<TScalar>
        where TScalarMultiplicativeSemigroup : ISemigroup<TScalar>
        where TVectorAddativeAbelianGroup : IAbelianGroup<TVector>
    {

        TScalarRing Scalar { get; }
        TVectorAddativeAbelianGroup Vector { get; }

        TVector Distribute(TScalar t, TVector r);
    }

    public interface IUnitaryModule<
        TScalar,
        TVector,
        TScalarRingWithUnity,
        TScalarAddativeGroup,
        TScalarMultiplicativeMonoid,
        TVectorAddativeAbelianGroup
    >
        : IModule<
            TScalar,
            TVector,
            TScalarRingWithUnity,
            TScalarAddativeGroup,
            TScalarMultiplicativeMonoid,
            TVectorAddativeAbelianGroup
        >
        where TScalarRingWithUnity : IRingWithUnity<TScalar, TScalarAddativeGroup, TScalarMultiplicativeMonoid>
        where TScalarAddativeGroup : IGroup<TScalar>
        where TScalarMultiplicativeMonoid : IMonoid<TScalar>
        where TVectorAddativeAbelianGroup : IAbelianGroup<TVector>
    {

    }

    public interface IVectorSpace<
        TScalar,
        TVector,
        TScalarField,
        TScalarAddativeAbelianGroup,
        TScalarMultiplicativeAbelianGroup,
        TVectorAddativeAbelianGroup
    >
        : IUnitaryModule<
            TScalar,
            TVector,
            TScalarField,
            TScalarAddativeAbelianGroup,
            TScalarMultiplicativeAbelianGroup,
            TVectorAddativeAbelianGroup
        >
        where TScalarField : IField<TScalar, TScalarAddativeAbelianGroup, TScalarMultiplicativeAbelianGroup>
        where TScalarAddativeAbelianGroup : IAbelianGroup<TScalar>
        where TScalarMultiplicativeAbelianGroup : IAbelianGroup<TScalar>
        where TVectorAddativeAbelianGroup : IAbelianGroup<TVector>
    {

    }

    public enum Symmetry { Rot000, Rot090, Rot180, Rot270, RefVer, RefDes, RefHoz, RefAsc }
    //VectorAbelianGroup
    public class SymmetryGroupoid : IGroupoid<Symmetry>
    {
        public Symmetry Operation(Symmetry a, Symmetry b)
        {
            // 64 cases
            return Symmetry.Rot000;
        }
    }


    public class VectorAbelianGroupoid : IGroupoid<RealVector>
    {
        public RealVector Operation(RealVector a, RealVector b)
        {
            if (a.Dimension != b.Dimension)
            {
                throw new Exception("a dimension not equal to b dimension");
            }

            RealVector r = new RealVector(a.Dimension);
            for (int i = 0; i < a.Dimension; i++)
            {
                r[i] = a[i] * b[i];
            }
            return r;
        }
    }

    public class SymmetrySemigroup : SymmetryGroupoid, ISemigroup<Symmetry>
    {

    }

    public class VectorAbelianSemigroup : VectorAbelianGroupoid, ISemigroup<RealVector>
    {

    }

    public class SymmetryMonoid : SymmetrySemigroup, IMonoid<Symmetry>
    {
        public Symmetry Identity
        {
            get { return Symmetry.Rot000; }
        }
    }

    public class VectorAbelianMonoid : VectorAbelianSemigroup, IMonoid<RealVector>
    {
        private RealVector m_Vector = new RealVector();

        public RealVector Identity
        {
            get { return m_Vector; }
        }
    }

    public class VectorAbelianGroup : VectorAbelianMonoid, IAbelianGroup<RealVector>
    {
        public RealVector Inverse(RealVector a)
        {
            RealVector r = new RealVector(a.Dimension);
            for (int i = 0; i < a.Dimension; i++)
            {
                r[i] = 1/a[i];
            }

            return r;
        }
    }
    public class SymmetryGroup : SymmetryMonoid, IGroup<Symmetry>
    {
        public Symmetry Inverse(Symmetry a)
        {
            switch (a)
            {
                case Symmetry.Rot000:
                    return Symmetry.Rot000;
                case Symmetry.Rot090:
                    return Symmetry.Rot270;
                case Symmetry.Rot180:
                    return Symmetry.Rot270;
                case Symmetry.Rot270:
                    return Symmetry.Rot090;
                case Symmetry.RefVer:
                    return Symmetry.RefVer;
                case Symmetry.RefDes:
                    return Symmetry.RefAsc;
                case Symmetry.RefHoz:
                    return Symmetry.RefHoz;
                case Symmetry.RefAsc:
                    return Symmetry.RefDes;
            }

            throw new NotImplementedException();
        }

    }

    public class AddativeIntegerGroupoid : IGroupoid<long>
    {
        public long Operation(long a, long b)
        {
            return a + b;
        }
    }

    public class AddativeRealGroupoid : IGroupoid<double>
    {
        public double Operation(double a, double b)
        {
            return a + b;
        }
    }

    public class AddativeIntegerSemigroup : AddativeIntegerGroupoid, ISemigroup<long>
    {

    }

    public class AddativeRealSemigroup : AddativeRealGroupoid, ISemigroup<double>
    {

    }

    public class AddativeIntegerMonoid : AddativeIntegerSemigroup, IMonoid<long>
    {
        public long Identity
        {
            get { return 0L; }
        }
    }

    public class AddativeRealMonoid : AddativeRealSemigroup, IMonoid<double>
    {
        public double Identity
        {
            get { return 0D; }
        }
    }

    public class AddativeIntegerGroup : AddativeIntegerMonoid, IGroup<long>
    {
        public long Inverse(long a)
        {
            return -a;
        }
    }

    public class AddativeRealGroup : AddativeRealMonoid, IGroup<double>
    {
        public double Inverse(double a)
        {
            return -a;
        }
    }

    public class AddativeIntegerAbelianGroup : AddativeIntegerGroup, IAbelianGroup<long>
    {

    }

    public class AddativeRealAbelianGroup : AddativeRealGroup, IAbelianGroup<double>
    {

    }

    public class MultiplicativeRealAbelianGroup : MultiplicativeRealGroup, IAbelianGroup<double>
    {

    }

    public class MultiplicativeIntegerGroupoid : IGroupoid<long>
    {
        public long Operation(long a, long b)
        {
            return a * b;
        }
    }

    public class MultiplicativeRealGroupoid : IGroupoid<double>
    {
        public double Operation(double a, double b)
        {
            return a * b;
        }
    }

    public class MultiplicativeIntegerSemigroup : MultiplicativeIntegerGroupoid, ISemigroup<long>
    {

    }

    public class MultiplicativeRealSemigroup : MultiplicativeRealGroupoid, ISemigroup<double>
    {

    }

    public class MultiplicativeIntegerMonoid : MultiplicativeIntegerSemigroup, IMonoid<long>
    {
        public long Identity
        {
            get { return 1L; }
        }
    }

    public class MultiplicativeRealMonoid : MultiplicativeRealSemigroup, IMonoid<double>
    {
        public double Identity
        {
            get { return 1D; }
        }
    }

    public class MultiplicativeRealGroup : MultiplicativeRealMonoid, IGroup<double>
    {
        public double Inverse(double a)
        {
            if(a == 0)
                throw new Exception("Non zero value");
            return 1/a;
        }
    }

    public class IntegerRingoid : IRingoid<long, AddativeIntegerGroupoid, MultiplicativeIntegerGroupoid>
    {
        public AddativeIntegerGroupoid Addition { get; private set; }
        public MultiplicativeIntegerGroupoid Multiplication { get; private set; }

        public IntegerRingoid()
        {
            Addition = new AddativeIntegerGroupoid();
            Multiplication = new MultiplicativeIntegerGroupoid();
        }

        public long Distribute(long a, long b)
        {
            return Multiplication.Operation(a, b);
        }
    }

    public class RealRingoid : IRingoid<double, AddativeRealGroupoid, MultiplicativeRealGroupoid>
    {
        public AddativeRealGroupoid Addition { get; private set; }
        public MultiplicativeRealGroupoid Multiplication { get; private set; }

        public RealRingoid()
        {
            Addition = new AddativeRealGroupoid();
            Multiplication = new MultiplicativeRealGroupoid();
        }

        public double Distribute(double a, double b)
        {
            return Multiplication.Operation(a, b);
        }
    }

    public class IntegerSemiring : IntegerRingoid, ISemiring<long, AddativeIntegerSemigroup, MultiplicativeIntegerSemigroup>
    {
        public new AddativeIntegerSemigroup  Addition { get; private set; }
        public new MultiplicativeIntegerSemigroup Multiplication { get; private set; }

        public IntegerSemiring()
            : base()
        {
            Addition = new AddativeIntegerSemigroup();
            Multiplication = new MultiplicativeIntegerSemigroup();
        }
    }

    public class RealSemiring : RealRingoid, ISemiring<double, AddativeRealSemigroup, MultiplicativeRealSemigroup>
    {
        public new AddativeRealSemigroup Addition { get; private set; }
        public new MultiplicativeRealSemigroup Multiplication { get; private set; }

        public RealSemiring()
            : base()
        {
            Addition = new AddativeRealSemigroup();
            Multiplication = new MultiplicativeRealSemigroup();
        }
    }

    public class IntegerRing : IntegerSemiring, IRing<long, AddativeIntegerGroup, MultiplicativeIntegerSemigroup>
    {
        public new AddativeIntegerGroup Addition { get; private set; }

        public IntegerRing()
            : base()
        {
            Addition = new AddativeIntegerGroup();
        }
    }

    public class RealRing : RealSemiring, IRing<double, AddativeRealGroup, MultiplicativeRealSemigroup>
    {
        public new AddativeRealGroup Addition { get; private set; }

        public RealRing()
            : base()
        {
            Addition = new AddativeRealGroup();
        }
    }

    public class IntegerRingWithUnity : IntegerRing, IRingWithUnity<long, AddativeIntegerGroup, MultiplicativeIntegerMonoid>
    {
        public new MultiplicativeIntegerMonoid Multiplication { get; private set; }

        public IntegerRingWithUnity()
            : base()
        {
            Multiplication = new MultiplicativeIntegerMonoid();
        }
    }

    public class RealRingWithUnity : RealRing, IRingWithUnity<double, AddativeRealGroup, MultiplicativeRealMonoid>
    {
        public new MultiplicativeRealMonoid Multiplication { get; private set; }

        public RealRingWithUnity()
            : base()
        {
            Multiplication = new MultiplicativeRealMonoid();
        }
    }

    public class RealField : IField<double, AddativeRealAbelianGroup, MultiplicativeRealAbelianGroup>
    {
        public AddativeRealAbelianGroup Addition { get; private set; }
        public MultiplicativeRealAbelianGroup Multiplication { get; private set; }

        public double Distribute(double a, double b)
        {
            return Multiplication.Operation(a, b);
        }

        public RealField()
            : base()
        {
            Addition = new AddativeRealAbelianGroup();
            Multiplication = new MultiplicativeRealAbelianGroup();
        }
    }

    public class RealVector
    {
        private List<double> vector = new List<double>();

        public int Dimension = 3;

        public RealVector()
        {
            vector = new List<double>(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                vector.Add(0D);
            }
        }

        public RealVector(int Dim)
        {
            Dimension = Dim;
            vector = new List<double>(Dimension);
            for (int i = 0; i < Dimension; i++)
            {
                vector.Add(0D);
            }
            
        }

        public double this[int n]
        {
            get { return vector[n]; }
            set { vector[n] = value; }
        }

    }

    public class Vector<T>
    {
        private T[] vector;

        public int Dimension
        {
            get { return vector.Length; }
        }

        public T this[int n]
        {
            get { return vector[n]; }
            set { vector[n] = value; }
        }

        public Vector()
        {
            vector = new T[2];
        }
    }

    public class RealVectorModule : IModule<double, RealVector, RealRing, AddativeRealGroup, MultiplicativeRealSemigroup, VectorAbelianGroup>
    {
        public RealRing Scalar
        {
            get;
            private set;
        }

        public VectorAbelianGroup Vector
        {
            get;
            private set;
        }

        public RealVectorModule()
        {
            Scalar = new RealRing();
            Vector = new VectorAbelianGroup();
        }

        public RealVector Distribute(double t, RealVector r)
        {
            RealVector c = new RealVector(r.Dimension);
            for (int i = 0; i < c.Dimension; i++)
                c[i] = Scalar.Multiplication.Operation(t, r[i]);
            return c;
        }
    }

    public class RealVectorUnitaryModule : RealVectorModule, IUnitaryModule<double, RealVector, RealRingWithUnity, AddativeRealGroup, MultiplicativeRealMonoid, VectorAbelianGroup>
    {
        public new RealRingWithUnity Scalar
        {
            get;
            private set;
        }

        public RealVectorUnitaryModule()
            : base()
        {
            Scalar = new RealRingWithUnity();
        }
    }

    public class RealVectorVectorSpace : RealVectorUnitaryModule, IVectorSpace<double, RealVector, RealField, AddativeRealAbelianGroup, MultiplicativeRealAbelianGroup, VectorAbelianGroup>
    {
        public new RealField Scalar
        {
            get;
            private set;
        }

        public RealVectorVectorSpace()
            : base()
        {
            Scalar = new RealField();
        }
    }

    public class StringGroupoid : IGroupoid<string>
    {
        public string Operation(String a, String b)
        {
            return string.Format("{0}{1}", a, b);
        }
    }

    public class StringSemigroup : StringGroupoid, ISemigroup<string>
    {

    }

    public class StringMonoid : StringSemigroup, IMonoid<string>
    {
        public string Identity
        {
            get { return string.Empty; }
        }
    }

    /*
    public class SequenceGroupoidWithUnity<Action> : IGroupoid<Action>
    {
        public Action Identity
        {
            get { return () => { }; }
        }

        public Action Operation(Action a, Action b) {
            return () => { a(); b(); };
    }
    }

    public class ChoiceGroupoid<Action> : IGroupoid<Action>
    {
        public Action Operation(Action a, Action b)
        {
            if (DateTime.Now.Ticks % 2 == 0)
                return a;
            return b;
        }
    }

    static public class GroupExtensions
    {
        static public T Sum<T>(this IEnumerable<T> E, IMonoid<T> m)
        {
            return E
                .FoldL(m.Identity, m.Operation);
        }
    }

    static public class RingoidExtensions
    {
        static public T Count<T, R, A, M>(this IEnumerable<R> E, IRingWithUnity<T, A, M> r)
            where A : IGroup<T>
            where M : IMonoid<T>
        {

            return E
                .Map((x) => r.Multiplication.Identity)
                .Sum(r.Addition);
        }

        static public T Mean<T, A, M>(this IEnumerable<T> E, IDivisionRing<T, A, M> r)
            where A : IGroup<T>
            where M : IGroup<T>
        {

            return r.Multiplication.Operation(
                r.Multiplication.Inverse(
                    E.Count(r)
                ),
                E.Sum(r.Addition)
            );
        }

        static public T Variance<T, A, M>(this IEnumerable<T> E, IDivisionRing<T, A, M> r)
            where A : IGroup<T>
            where M : IGroup<T>
        {

            T average = E.Mean(r);

            return r.Multiplication.Operation(
                r.Multiplication.Inverse(
                    E.Count(r)
                ),
                E
                    .Map((x) => r.Addition.Operation(x, r.Addition.Inverse(average)))
                    .Map((x) => r.Multiplication.Operation(x, x))
                    .Sum(r.Addition)
            );
        }
    }

    
    static public class ModuleExtensions
    {
        static public TV Mean<TS, TV, TSR, TSRA, TSRM, TVA>(this IEnumerable<TV> E, IVectorField<TS, TV, TSR, TSRA, TSRM, TVA> m)
            where TSR : IField<TS, TSRA, TSRM>
            where TSRA : IAbelianGroup<TS>
            where TSRM : IAbelianGroup<TS>
            where TVA : IAbelianGroup<TV>
        {

            return m.Distribute(
                m.Scalar.Multiplication.Inverse(
                    E.Count(m.Scalar)
                ),
                E.FoldL(
                    m.Vector.Identity,
                    m.Vector.Operation
                )
            );
        }
    }
*/
    
}
