﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver.Linq.Expressions
{
    internal class OrderByExpression : ExtensionExpression
    {
        private readonly Expression _source;
        private readonly ReadOnlyCollection<SortClause> _clauses;

        public OrderByExpression(Expression source, IEnumerable<SortClause> clauses)
        {
            _source = Ensure.IsNotNull(source, "source");
            _clauses = Ensure.IsNotNull(clauses, "clauses") as ReadOnlyCollection<SortClause>;
            if (_clauses == null)
            {
                _clauses = new List<SortClause>(clauses).AsReadOnly();
            }
        }

        public ReadOnlyCollection<SortClause> Clauses
        {
            get { return _clauses; }
        }

        public Expression Source
        {
            get { return _source; }
        }

        public override ExtensionExpressionType ExtensionType
        {
            get { return ExtensionExpressionType.OrderBy; }
        }

        public override string ToString()
        {
            var clauseStrings = string.Join(", ", _clauses.Select(c => c.ToString()));
            return string.Format("{0}.OrderBy({1})", _source.ToString(), clauseStrings);
        }

        public OrderByExpression Update(Expression source, ReadOnlyCollection<SortClause> clauses)
        {
            if (source != _source ||
                clauses != _clauses)
            {
                return new OrderByExpression(source, clauses);
            }

            return this;
        }

        protected internal override Expression Accept(ExtensionExpressionVisitor visitor)
        {
            return visitor.VisitOrderBy(this);
        }
    }
}