//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



namespace Bio
{
    /// <summary>
    /// Class to hold updated sequence item and type of update.
    /// </summary>
    public class UpdatedSequenceItem
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UpdatedSequenceItem() { }

        /// <summary>
        /// Creates UpdatedSequenceItem from the specified updatedSequenceItem.
        /// </summary>
        /// <param name="updatedSequenceItem">UpdatedSequenceItem instance.</param>
        public UpdatedSequenceItem(UpdatedSequenceItem updatedSequenceItem)
        {
            if (updatedSequenceItem != null)
            {
                SequenceItem = updatedSequenceItem.SequenceItem;
                Type = updatedSequenceItem.Type;
            }
        }

        /// <summary>
        /// Creates UpdatedSequenceItem from the specified sequence item and update type.
        /// </summary>
        /// <param name="sequenceItem">Sequence item.</param>
        /// <param name="type">Update type.</param>
        public UpdatedSequenceItem(ISequenceItem sequenceItem, UpdateType type)
        {
            SequenceItem = sequenceItem;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the updated sequence item.
        /// </summary>
        public ISequenceItem SequenceItem { get; set; }

        /// <summary>
        /// Gets or sets the update type.
        /// </summary>
        public UpdateType Type { get; set; }
    }
}
