library VectorMath /* v1.01


    */uses /*

    */Table         /*  https://www.hiveworkshop.com/threads/snippet-new-table.188084/
    */NxList        /*  https://github.com/nestharus/JASS/blob/master/jass/Data%20Structures/NxList/script.j
    */ErrorMessage  /*  https://github.com/nestharus/JASS/blob/master/jass/Systems/ErrorMessage/main.j

    Resource Link:      https://www.hiveworkshop.com/threads/snippet-vectormath.307136/


    *///! novjass

    /*
    Description:

        A Vector is a 1x3 matrix representation of... anything! But it is usually used in mathematics to
        represent something that have components in 3D space (x, y, and z) such as a point, line, force,
        velocity, accleration, etc.

        This snippet is a reinvention of Anitarf's Vector library. This attempts to provide more functionality
        and a better API to the users. The old Vector library also have redundant codes and in most parts, does
        not adhere to the concept of DRY (Do not Repeat Yourself) in programming, making its script significantly
        longer than it ought to be.

        One important key feature that this snippet has is 'hooks'. Basically, it allows you to turn a Vector
        into a function of one or more Vectors. This can be used to easily describe relative motions such as
        Relative Orbital Motions (Ex: The moon is orbiting the Earth while the Earth itself is also orbiting the
        Sun, while the Sun is also orbiting around another object) and many more.

    */

    |=====|
    | API |
    |=====|
    /*
      */struct Vector/*

          */static constant Vector NULL     /*  Vector(0)

            - Constant unit vectors:
          */static constant Vector X_AXIS   /*
          */static constant Vector Y_AXIS   /*
          */static constant Vector Z_AXIS   /*

            - Fields:
          */real x                          /*
          */real y                          /*
          */real z                          /*
          */real magnitude                  /*
          */boolean zero                    /*  Checks if the vector has zero magnitude
          */debug boolean constant          /*  Checks if the vector is one of the constant unit vectors
          */debug boolean allocated         /*

            - Methods: You can append a negative (-) sign to the vector arguments to temporarily inverse them inside the
                       methods they are passed into, but you can't do this to the vector instance for whom the method is called.
                       For example, if you want to get the difference between two vectors, you can do "Vector.sum(vecA, -vecB)".
          */static method   create              takes real x, real y, real z                            returns Vector/*
          */method          destroy             takes nothing                                           returns nothing/*
            - Constructor/Destructor

          */static method   operator []         takes Vector whichVector                                returns Vector/*
            - Copy Constructor
          */static method   operator []=        takes Vector destination, Vector source                 returns nothing/*
            - Overwrite Operator
          */method          operator ==         takes Vector whichVector                                returns boolean/*
          */method          operator !=         takes Vector whichVector                                returns boolean/*
          */method          operator <          takes Vector whichVector                                returns boolean/*
          */method          operator >          takes Vector whichVector                                returns boolean/*
            - Relational Operators
            - The == and != operators check if the two vectors have the same components
            - The < and > operators compares the magnitude of the two vectors

          */static method   getAngle            takes Vector vecA, Vector vecB                          returns real/*
            - Returns the angle between two vectors in radians

          */static method   sum                 takes Vector vecA, Vector vecB                          returns Vector/*
          */method          add                 takes Vector whichVector                                returns this/*

          */static method   getScaled           takes Vector whichVector, real scaleValue               returns Vector/*
          */method          scale               takes real scaleValue                                   returns this/*

          */static method   getDirection        takes nothing                                           returns Vector/*
            - Returns the vector's unit vector
          */method          setDirection        takes Vector whichVector                                returns this/*
            - <whichVector> need not be a unit vector

          */static method   getRotated          takes Vector whichVector, Vector axis, real radians     returns Vector/*
          */method          rotate              takes Vector axis, real radians                         returns this/*

          */static method   inverse             takes Vector whichVector                                returns Vector/*
            - Returns the negative of this vector as a new vector
          */method          invert              takes nothing                                           returns this/*
            - Turns this vector into its negative

          */static method   scalarProduct       takes Vector vecA, Vector vecB                          returns real/*
            - Performs a dot product between two vectors (vecA.vecB)
          */static method   vectorProduct       takes Vector vecA, Vector vecB                          returns Vector/*
            - Performs a cross product between two vectors (vecA x vecB)

          */static method   scalarTripleProduct takes Vector vecA, Vector vecB, Vector vecC             returns real/*
            - Returns (vecA x vecB . vecC)
          */static method   vectorTripleProduct takes Vector vecA, Vector vecB, Vector vecC             returns Vector/*
            - Returns (vecA x vecB x vecC)

          */static method   vectorProjection    takes Vector whichVector, Vector direction              returns Vector/*
          */method          projectToVector     takes Vector direction                                  returns this/*
            - Direction vector must not be zero

          */static method   planeProjection     takes Vector whichVector, Vector normal                 returns Vector/*
          */method          projectToPlane      takes Vector normal                                     returns this/*
            - Normal vector must not be zero

          */method          hook                takes Vector whichVector                                returns this/*
          */method          unhook              takes Vector whichVector                                returns this/*
          */method          clearHooks          takes nothing                                           returns this/*
          */method          clearLinks          takes nothing                                           returns this/*
            - Hooking a vector causes this vector to be dependent on the properties of the hooked vector, turning this vector
              into a function of another vector (or vectors, since you can hook multiple vectors)
            - In other words, any modification on the hooked vector will also modify this vector
            - Vector(this).x = this.x + hookedVec[1].x + ... + hookedVec[N].x (Where 'this.x' is this vector's 'own x')
            - clearHooks() unhooks all of this vector's hooked vectors
            - clearLinks() unhooks this vector from all of its hookers


    *///! endnovjass

    private module Init
        private static method onInit takes nothing returns nothing
            call initTables()
            call initConstantVectors()
            call initObjectStack()
        endmethod
    endmodule

    private struct Hook extends array
        implement NxList
        Vector data
    endstruct

    private struct Link extends array
        implement NxList
        Vector data
    endstruct

    struct Vector extends array

        private static TableArray hookTable
        private static TableArray linkTable
        private static TableArray table
        private static integer array recycler

        private real xComponent
        private real yComponent
        private real zComponent

        static constant method operator NULL takes nothing returns thistype
            return 0
        endmethod
        static constant method operator X_AXIS takes nothing returns thistype
            return 1
        endmethod
        static constant method operator Y_AXIS takes nothing returns thistype
            return 2
        endmethod
        static constant method operator Z_AXIS takes nothing returns thistype
            return 3
        endmethod

        debug method operator allocated takes nothing returns boolean
            debug return recycler[this] == -1
        debug endmethod
        debug method operator constant takes nothing returns boolean
            debug return (this) == (X_AXIS) or (this) == (Y_AXIS) or (this) == (Z_AXIS)
        debug endmethod

        private method operator sign takes nothing returns integer
            if this < 0 then
                return -1
            endif
            return 1
        endmethod

        private method updateX takes real value returns nothing
            local real dx = value - this.xComponent
            local Link link = Link(this).first
            loop
                exitwhen link == 0
                set link.data.xComponent = link.data.xComponent + dx
                set link = link.next
            endloop
            set this.xComponent = value
        endmethod
        private method updateY takes real value returns nothing
            local real dy = value - this.yComponent
            local Link link = Link(this).first
            loop
                exitwhen link == 0
                set link.data.yComponent = link.data.yComponent + dy
                set link = link.next
            endloop
            set this.yComponent = value
        endmethod
        private method updateZ takes real value returns nothing
            local real dz = value - this.zComponent
            local Link link = Link(this).first
            loop
                exitwhen link == 0
                set link.data.zComponent = link.data.zComponent + dz
                set link = link.next
            endloop
            set this.zComponent = value
        endmethod

        method operator x= takes real value returns nothing
            debug call ThrowError(this.constant,            "VectorMath", "x=", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "x=", "thistype", this, "Attempted to use an unallocated instance")
            call this.updateX(value)
        endmethod
        method operator x takes nothing returns real
            debug call ThrowError(not this.allocated,       "VectorMath", "x", "thistype", this, "Attempted to use an unallocated instance")
            return this.xComponent
        endmethod

        method operator y= takes real value returns nothing
            debug call ThrowError(this.constant,            "VectorMath", "y=", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "y=", "thistype", this, "Attempted to use an unallocated instance")
            call this.updateY(value)
        endmethod
        method operator y takes nothing returns real
            debug call ThrowError(not this.allocated,       "VectorMath", "y", "thistype", this, "Attempted to use an unallocated instance")
            return this.yComponent
        endmethod

        method operator z= takes real value returns nothing
            debug call ThrowError(this.constant,            "VectorMath", "z=", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "z=", "thistype", this, "Attempted to use an unallocated instance")
            call this.updateZ(value)
        endmethod
        method operator z takes nothing returns real
            debug call ThrowError(not this.allocated,       "VectorMath", "x=", "thistype", this, "Attempted to use an unallocated instance")
            return this.zComponent
        endmethod

        static method create takes real x, real y, real z returns thistype
            local thistype this = recycler[0]
            debug call ThrowError(this == 0,                "VectorMath", "create()", "thistype", 0, "Overflow")
            set recycler[0] = recycler[this]
            debug set recycler[this] = -1
            set this.xComponent = x
            set this.yComponent = y
            set this.zComponent = z
            call Hook(this).clear()
            call Link(this).clear()
            return this
        endmethod

        method update takes real x, real y, real z returns thistype
            local real dx
            local real dy
            local real dz
            local thistype data
            local Link link = Link(this).first
            debug call ThrowError(this.constant,            "VectorMath", "update()", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "update()", "thistype", this, "Attempted to use an unallocated instance")
            if link != 0 then
                set dx = x - this.x
                set dy = y - this.y
                set dz = z - this.z
                loop
                    exitwhen link == 0
                    set data = link.data
                    set data.xComponent = data.x + dx
                    set data.yComponent = data.y + dy
                    set data.zComponent = data.z + dz
                    set link = link.next
                endloop
            endif
            set this.xComponent = x
            set this.yComponent = y
            set this.zComponent = z
            return this
        endmethod

        static method operator [] takes thistype vec returns thistype
            local integer sign = vec.sign
            set vec = vec*sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "operator[]", "thistype", vec, "Attempted to use an unallocated instance")
            return create(vec.x*sign, vec.y*sign, vec.z*sign)
        endmethod
        static method operator []= takes thistype this, thistype vec returns nothing
            local integer sign = vec.sign
            set vec = vec*sign
            debug call ThrowError(not this.allocated,       "VectorMath", "operator[]=", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not vec.allocated,        "VectorMath", "operator[]=", "thistype", vec, "Attempted to use an unallocated instance")
            call this.update(vec.x*sign, vec.y*sign, vec.z*sign)
        endmethod

        method operator zero takes nothing returns boolean
            debug call ThrowError(not this.allocated,       "VectorMath", "zero", "thistype", this, "Attempted to use an unallocated instance")
            return not (this.x != 0.00 or this.y != 0.00 or this.z != 0.00)
        endmethod

        static method sum takes thistype a, thistype b returns thistype
            local integer signA = a.sign
            local integer signB = b.sign
            set a = a*signA
            set b = b*signB
            debug call ThrowError(not a.allocated,          "VectorMath", "sum()", "thistype", a, "Attempted to use an unallocated instance")
            debug call ThrowError(not b.allocated,          "VectorMath", "sum()", "thistype", b, "Attempted to use an unallocated instance")
            return create(a.x*signA + b.x*signB, a.y*signA + b.y*signB, a.z*signA + b.z*signB)
        endmethod
        method add takes thistype vec returns thistype
            local integer sign = vec.sign
            set vec = vec*sign
            debug call ThrowError(this.constant,            "VectorMath", "add()", "thistype", this, "Attempted to edit an attribute of a constant vector")
            return this.update(this.x + vec.x*sign, this.y + vec.y*sign, this.z + vec.z*sign)
        endmethod

        static method getScaled takes thistype vec, real factor returns thistype
            local integer sign = vec.sign
            set vec = vec*sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "getScaled()", "thistype", vec, "Attempted to use an unallocated instance")
            set factor = factor*sign
            return create(vec.x*factor, vec.y*factor, vec.z*factor)
        endmethod
        method scale takes real factor returns thistype
            debug call ThrowError(this.constant,            "VectorMath", "scale()", "thistype", this, "Attempted to scale a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "scale()", "thistype", this, "Attempted to use an unallocated instance")
            return this.update(this.x*factor, this.y*factor, this.z*factor)
        endmethod

        method operator magnitude takes nothing returns real
            debug call ThrowError(not this.allocated,       "VectorMath", "magnitude", "thistype", this, "Attempted to use an unallocated instance")
            return SquareRoot(this.x*this.x + this.y*this.y + this.z*this.z)
        endmethod
        method operator magnitude= takes real value returns nothing
            debug call ThrowError(this.constant,            "VectorMath", "magnitude=", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "magnitude=", "thistype", this, "Attempted to use an unallocated instance")
            call this.scale(value/this.magnitude)
        endmethod

        method operator == takes thistype vec returns boolean
            local integer signA = this.sign
            local integer signB = vec.sign
            set this = this*signA
            set vec = vec*signB
            debug call ThrowError(not this.allocated,       "VectorMath", "operator==", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not vec.allocated,        "VectorMath", "operator==", "thistype", this, "Attempted to use an unallocated instance")
            return this.x*signA == vec.x*signB and this.y*signA == vec.y*signB and this.z*signA == vec.z*signB
        endmethod
        method operator < takes thistype vec returns boolean
            set this = this*this.sign
            set vec = vec*vec.sign
            debug call ThrowError(not this.allocated,       "VectorMath", "operator<", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not vec.allocated,        "VectorMath", "operator<", "thistype", this, "Attempted to use an unallocated instance")
            return this.x*this.x + this.y*this.y + this.z*this.z < vec.x*vec.x + vec.y*vec.y + vec.z*vec.z
        endmethod

        method getDirection takes nothing returns thistype
            local real magnitude = this.magnitude
            debug call ThrowError(not this.allocated,       "VectorMath", "getDirection()", "thistype", this, "Attempted to use an unallocated instance")
            return create(this.x/magnitude, this.y/magnitude, this.z/magnitude)
        endmethod

        method setDirection takes thistype direction returns thistype
            local integer sign = direction.sign
            local real magnitude = this.magnitude
            set direction = direction*sign
            debug call ThrowError(this.constant,            "VectorMath", "setDirection()", "thistype", this, "Attempted to edit an attribute of a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "setDirection()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not direction.allocated,  "VectorMath", "setDirection()", "thistype", direction, "Attempted to use an unallocated instance")
            debug call ThrowError(direction.zero,           "VectorMath", "setDirection()", "thistype", direction, "The direction vector is zero")
            call this.update(direction.x*sign, direction.y*sign, direction.z*sign)
            set this.magnitude = magnitude
            return this
        endmethod

        static method inverse takes thistype vec returns thistype
            local integer sign = -vec.sign
            set vec = -vec*sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "inverse()", "thistype", vec, "Attempted to use an unallocated instance")
            return create(vec.x*sign, vec.y*sign, vec.z*sign)
        endmethod
        method invert takes nothing returns thistype this
            debug call ThrowError(this.constant,            "VectorMath", "invert()", "thistype", this, "Attempted to invert a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "invert()", "thistype", this, "Attempted to use an unallocated instance")
            return this.scale(-1.00)
        endmethod

        static method scalarProduct takes thistype a, thistype b returns real
            local integer signA = a.sign
            local integer signB = b.sign
            set a = a*signA
            set b = b*signB
            debug call ThrowError(not a.allocated,          "VectorMath", "scalarProduct()", "thistype", a, "Attempted to use an unallocated instance")
            debug call ThrowError(not b.allocated,          "VectorMath", "scalarProduct()", "thistype", b, "Attempted to use an unallocated instance")
            return (a.x*b.x + a.y*b.y + a.z*b.z)*signA*signB
        endmethod
        static method vectorProduct takes thistype a, thistype b returns thistype
            local integer signA = a.sign
            local integer signB = b.sign
            local integer sign = signA*signB
            set a = a*signA
            set b = b*signB
            debug call ThrowError(not a.allocated,          "VectorMath", "vectorProduct()", "thistype", a, "Attempted to use an unallocated instance")
            debug call ThrowError(not b.allocated,          "VectorMath", "vectorProduct()", "thistype", b, "Attempted to use an unallocated instance")
            return create((a.y*b.z - a.z*b.y)*sign, (a.z*b.x - a.x*b.z)*sign, (a.x*b.y - a.y*b.x)*sign)
        endmethod

        static method scalarTripleProduct takes thistype a, thistype b, thistype c returns real
            debug call ThrowError(recycler[a*a.sign] != -1, "VectorMath", "scalarTripleProduct()", "thistype", a*a.sign, "Attempted to use an unallocated instance")
            debug call ThrowError(recycler[b*b.sign] != -1, "VectorMath", "scalarTripleProduct()", "thistype", b*b.sign, "Attempted to use an unallocated instance")
            debug call ThrowError(recycler[c*c.sign] != -1, "VectorMath", "scalarTripleProduct()", "thistype", c*c.sign, "Attempted to use an unallocated instance")
            return scalarProduct(vectorProduct(a, b), c)
        endmethod
        static method vectorTripleProduct takes thistype a, thistype b, thistype c returns thistype
            local real m
            local real n
            local integer signB = b.sign
            local integer signC = c.sign
            set a = a*a.sign
            set b = b*signB
            set c = c*signC
            debug call ThrowError(not a.allocated,          "VectorMath", "vectorTripleProduct()", "thistype", a, "Attempted to use an unallocated instance")
            debug call ThrowError(not b.allocated,          "VectorMath", "vectorTripleProduct()", "thistype", b, "Attempted to use an unallocated instance")
            debug call ThrowError(not c.allocated,          "VectorMath", "vectorTripleProduct()", "thistype", c, "Attempted to use an unallocated instance")
            set m = (a.x*b.x + a.y*b.y + a.z*b.z)*signC
            set n = (a.x*c.x + a.y*c.y + a.z*c.z)*signB
            return create(b.x*n - c.x*m, b.y*n - c.y*m, b.z*n - c.z*m)
        endmethod

        static method getAngle takes thistype a, thistype b returns real
            debug set a = a*a.sign
            debug set b = b*b.sign
            debug call ThrowError(not a.allocated,          "VectorMath", "getAngle()", "thistype", a, "Attempted to use an unallocated instance")
            debug call ThrowError(not b.allocated,          "VectorMath", "getAngle()", "thistype", b, "Attempted to use an unallocated instance")
            debug call ThrowError(a.zero or b.zero,         "VectorMath", "getAngle()", "thistype", 0, "Atleast one of the vector is zero")
            return Acos(scalarProduct(a, b)/(thistype(a*a.sign).magnitude*thistype(b*b.sign).magnitude))
        endmethod

        method projectToVector takes thistype direction returns thistype
            local real square
            set direction = direction*direction.sign
            debug call ThrowError(this.constant,            "VectorMath", "projectToVector()", "thistype", this, "Attempted to project a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "projectToVector()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not direction.allocated,  "VectorMath", "projectToVector()", "thistype", direction, "Attempted to use an unallocated instance")
            debug call ThrowError(direction.zero,           "VectorMath", "projectToVector()", "thistype", direction, "The direction vector is zero")
            set square = ((this.x*direction.x + this.y*direction.y + this.z*direction.z)/(direction.x*direction.x + direction.y*direction.y + direction.z*direction.z))*direction.sign
            return this.update(direction.x*square, direction.y*square, direction.z*square)
        endmethod
        method projectToPlane takes thistype normal returns thistype
            local real l
            set normal = normal*normal.sign
            set l = (this.x*normal.x + this.y*normal.y + this.z*normal.z)/(normal.x*normal.x + normal.y*normal.y + normal.z*normal.z)
            debug call ThrowError(this.constant,            "VectorMath", "projectToPlane()", "thistype", this, "Attempted to project a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "projectToPlane()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not normal.allocated,     "VectorMath", "projectToPlane()", "thistype", normal, "Attempted to use an unallocated instance")
            debug call ThrowError(normal.zero,              "VectorMath", "projectToPlane()", "thistype", normal, "The normal vector is zero")
            return this.update(this.x - normal.x*l, this.y - normal.y*l, this.z - normal.z*l)
        endmethod

        static method vectorProjection takes thistype vec, thistype direction returns thistype
            debug set vec = vec*vec.sign
            debug set direction = direction*direction.sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "vectorProjection()", "thistype", vec, "Attempted to use an unallocated instance")
            debug call ThrowError(not direction.allocated,  "VectorMath", "vectorProjection()", "thistype", direction, "Attempted to use an unallocated instance")
            debug call ThrowError(direction.zero,           "VectorMath", "vectorProjection()", "thistype", direction, "The direction vector is zero")
            return thistype[vec].projectToVector(direction)
        endmethod
        static method planeProjection takes thistype vec, thistype normal returns thistype
            debug set vec = vec*vec.sign
            debug set normal = normal*normal.sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "planeProjection()", "thistype", vec, "Attempted to use an unallocated instance")
            debug call ThrowError(not normal.allocated,     "VectorMath", "planeProjection()", "thistype", normal, "Attempted to use an unallocated instance")
            debug call ThrowError(normal.zero,              "VectorMath", "planeProjection()", "thistype", normal, "The normal vector is zero")
            return thistype[vec].projectToPlane(normal)
        endmethod

        method rotate takes thistype axis, real rad returns thistype
            local real xx
            local real xy
            local real xz
            local real zx
            local real zy
            local real zz
            local real al
            local real factor
            local real cos = Cos(rad)
            local real sin = Sin(rad)
            local integer sign = axis.sign
            set axis = axis*sign
            set al = (axis.x*axis.x + axis.y*axis.y + axis.z*axis.z)*sign
            debug call ThrowError(this.constant,            "VectorMath", "rotate()", "thistype", this, "Attempted to rotate a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "rotate()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not axis.allocated,       "VectorMath", "rotate()", "thistype", axis, "Attempted to use an unallocated instance")
            debug call ThrowError(axis.zero,                "VectorMath", "rotate()", "thistype", axis, "The axis vector is zero")
            set factor = (scalarProduct(this, axis)/al)
            set zx = axis.x*factor
            set zy = axis.y*factor
            set zz = axis.z*factor
            set xx = this.x - zx
            set xy = this.y - zy
            set xz = this.z - zz
            set al = SquareRoot(al)
            return this.update(xx*cos + ((axis.y*xz - axis.z*xy)/al)*sin + zx, /*
                             */xy*cos + ((axis.z*xx - axis.x*xz)/al)*sin + zy, /*
                             */xz*cos + ((axis.x*xy - axis.y*xx)/al)*sin + zz)
        endmethod
        static method getRotated takes thistype vec, thistype axis, real rad returns thistype
            debug set vec = vec*vec.sign
            debug set axis = axis*axis.sign
            debug call ThrowError(not vec.allocated,        "VectorMath", "getRotated()", "thistype", vec, "Attempted to use an unallocated instance")
            debug call ThrowError(not axis.allocated,       "VectorMath", "getRotated()", "thistype", axis, "Attempted to use an unallocated instance")
            debug call ThrowError(axis.zero,                "VectorMath", "getRotated()", "thistype", axis, "The axis vector is zero")
            return thistype[vec].rotate(axis, rad)
        endmethod

        private method joint takes thistype vec returns nothing
            local Hook hook = Hook(this).enqueue()
            local Link link = Link(vec).enqueue()
            set hook.data = vec
            set link.data = this
            set hookTable[this][vec] = hook
            set linkTable[vec][this] = link
        endmethod
        private method unjoint takes thistype vec returns nothing
            call Hook(hookTable[this][vec]).remove()
            call Link(linkTable[vec][this]).remove()
            call hookTable[this].remove(vec)
            call linkTable[vec].remove(this)
        endmethod

        private method setupHook takes thistype vec returns nothing
            local integer count = table[this][vec]
            if count == 0 then
                call this.joint(vec)
            endif
            set table[this][vec] = count + 1
        endmethod
        private method destroyHook takes thistype vec returns nothing
            local integer count = table[this][vec] - 1
            set table[this][vec] = count
            if count == 0 then
                call this.unjoint(vec)
            endif
        endmethod

        private method setupHooksToList takes thistype vec returns nothing
            local Hook node = Hook(vec).first
            local thistype data
            local integer count
            loop
                exitwhen node == 0
                set data = node.data
                set count = table[this][data]
                if count == 0 then
                    call this.joint(data)
                endif
                set table[this][data] = count + table[vec][data]
                set node = node.next
            endloop
        endmethod
        private method destroyHooksToList takes thistype vec returns nothing
            local Hook node = Hook(vec).first
            local Hook nextNode
            local thistype data
            local integer count
            loop
                exitwhen node == 0
                set nextNode = node.next
                set data = node.data
                set count = table[this][data] - table[vec][data]
                if count == 0 then
                    call this.unjoint(data)
                endif
                set node = nextNode
            endloop
        endmethod

        method hook takes thistype vec returns thistype
            local Link node = Link(this).first
            debug set vec = vec*vec.sign
            debug call ThrowError(this.constant,            "VectorMath", "hook()", "thistype", this, "Attempted to hook using a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "hook()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not vec.allocated,        "VectorMath", "hook()", "thistype", vec, "Attempted to use an unallocated instance")
            debug call ThrowError(hookTable[this].has(vec), "VectorMath", "hook()", "thistype", this, "Attempted to hook an already hooked Vector")
            call this.setupHook(vec)
            call this.setupHooksToList(vec)
            loop
                exitwhen node == 0
                call node.data.setupHook(vec)
                call node.data.setupHooksToList(vec)
                set node = node.next
            endloop
            return this.add(vec)
        endmethod
        method unhook takes thistype vec returns thistype
            local Link node = Link(this).first
            local Link nextNode
            debug set vec = vec*vec.sign
            debug call ThrowError(this.constant,            "VectorMath", "unhook()", "thistype", this, "Attempted to unhook using a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "unhook()", "thistype", this, "Attempted to use an unallocated instance")
            debug call ThrowError(not vec.allocated,        "VectorMath", "unhook()", "thistype", vec, "Attempted to use an unallocated instance")
            debug call ThrowError(not hookTable[this].has(vec), "VectorMath", "unhook()", "thistype", this, "Attempted to unhook an unhooked Vector")
            call this.add(-vec)
            loop
                exitwhen node == 0
                set nextNode = node.next
                call node.data.destroyHooksToList(vec)
                call node.data.destroyHook(vec)
                set node = nextNode
            endloop
            call this.destroyHooksToList(vec)
            call this.destroyHook(vec)
            return this
        endmethod

        method clearHooks takes nothing returns thistype
            local Hook node = Hook(this).first
            local Hook nextNode
            debug call ThrowError(not this.allocated,       "VectorMath", "clearHooks()", "thistype", this, "Attempted to use an unallocated instance")
            loop
                exitwhen node == 0
                set nextNode = node.next
                call this.unhook(node.data)
                set node = nextNode
            endloop
            return this
        endmethod
        method clearLinks takes nothing returns thistype
            local Link node = Link(this).first
            local Link nextNode
            debug call ThrowError(not this.allocated,       "VectorMath", "clearLinks()", "thistype", this, "Attempted to use an unallocated instance")
            loop
                exitwhen node == 0
                set nextNode = node.next
                call node.data.unhook(this)
                set node = nextNode
            endloop
            return this
        endmethod

        method destroy takes nothing returns nothing
            debug call ThrowError(this.constant,            "VectorMath", "destroy()", "thistype", this, "Attempted to destroy a constant vector")
            debug call ThrowError(not this.allocated,       "VectorMath", "destroy()", "thistype", this, "Double-free")
            call this.clearHooks()
            call this.clearLinks()
            call Hook(this).destroy()
            call Link(this).destroy()
            set this.xComponent = 0.00
            set this.yComponent = 0.00
            set this.zComponent = 0.00
            set recycler[this] = recycler[0]
            set recycler[0] = this
        endmethod

        private static method initConstantVectors takes nothing returns nothing
            set X_AXIS.xComponent = 1.00
            set Y_AXIS.yComponent = 1.00
            set Z_AXIS.zComponent = 1.00
            debug set recycler[X_AXIS] = -1
            debug set recycler[Y_AXIS] = -1
            debug set recycler[Z_AXIS] = -1
        endmethod

        private static method initObjectStack takes nothing returns nothing
            local integer node = 4
            local integer maxIndex = JASS_MAX_ARRAY_SIZE - 2
            set recycler[maxIndex] = 0
            set recycler[0] = node
            loop
                exitwhen node == maxIndex
                set recycler[node] = node + 1
                set node = node + 1
            endloop
        endmethod

        private static method initTables takes nothing returns nothing
            set hookTable   = TableArray[JASS_MAX_ARRAY_SIZE - 1]
            set linkTable   = TableArray[JASS_MAX_ARRAY_SIZE - 1]
            set table       = TableArray[JASS_MAX_ARRAY_SIZE - 1]
        endmethod

        implement Init

    endstruct


endlibrary