using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Annotation
    {
        public int Id_Art;
        public int AnnotationNumber;
        public int AnnotationId;

        public Annotation(int id_art, int num, int id_ann)
        {
            this.Id_Art = id_art;
            this.AnnotationId = id_ann;
            this.AnnotationNumber = num;
        }
    }
}
