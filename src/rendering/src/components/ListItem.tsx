import { Field } from '@sitecore-jss/sitecore-jss-nextjs';
import { StyleguideComponentProps } from 'lib/component-props';

type ListItemProps = StyleguideComponentProps & {
  fields: {
    text: Field<string>;
    textColor: Field<string>;
  };
};

const ListItem = (props: ListItemProps): JSX.Element => {
  if (props.fields.textColor?.value) {
    return <li><span style={{color: props.fields.textColor.value}}>{props.fields.text.value}</span></li>
  }
  else {
    return <li>{props.fields.text.value}</li>
  }
};

export default ListItem;
