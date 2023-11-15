import { Validation } from './validation';

export const Messages = {
  TopicIsRequired: 'Topic name is required.',
  TopicIsTooShort: `Topic has to be at least ${Validation.MinTopicLength} characters long.`,
  TopicIsTooLong: `Topic has to be maximum ${Validation.MaxTopicLength} characters long.`,

  NameIsRequired: 'Name is required.',
  NameIsTooShort: `Name has to be at least ${Validation.MinNameLength} characters long.`,
  NameIsTooLong: `Name has to be maximum ${Validation.MaxNameLength} characters long.`,
} as const;
