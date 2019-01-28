﻿using QRest.Core.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QRest.Core.Terms
{
    public class SequenceTerm : IEnumerable<ITerm>, ITerm
    {
        private readonly LinkedList<ITerm> _sequence = new LinkedList<ITerm>();

        public SequenceTerm(params ITerm[] terms)
        {
            Add(terms);

            SharedView = $"{string.Join("", _sequence.Select(t => t.SharedView))}";
            KeyView = string.Join("", _sequence.Select(t => t.KeyView));
            DebugView = $"#{string.Join("", _sequence.Select(t => t.DebugView))}";
        }

        public ITerm Root => _sequence.First.Value;
        public ITerm Last => _sequence.Last.Value;
        public bool IsEmpty => !_sequence.Any();

        public virtual string SharedView { get; protected set; }
        public virtual string KeyView { get; protected set; }
        public virtual string DebugView { get; protected set; }

        public SequenceTerm Append(params ITerm[] terms)
        {
            var clone = (SequenceTerm)Clone();
            clone.Add(terms);

            return clone;
        }

        protected void Add(ITerm term)
        {
            if (term is SequenceTerm s)
            {
                if (term.GetType() != typeof(SequenceTerm))
                    throw new System.InvalidOperationException("This sequence cannot be liniarized.");

                Add((IEnumerable<ITerm>)s);
            }
            else if (term != null)
                _sequence.AddLast(term);
        }

        protected void Add(IEnumerable<ITerm> terms)
        {
            foreach (var term in terms.Where(t => t != null))
                Add(term);
        }

        public IEnumerator<ITerm> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }

        public virtual ITerm Clone() => new SequenceTerm(_sequence.Select(t => t.Clone()).ToArray());

        public override string ToString() => SharedView;
    }
}
